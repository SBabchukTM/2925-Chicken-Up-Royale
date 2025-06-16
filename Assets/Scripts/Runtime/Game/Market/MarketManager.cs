using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Care;
using Runtime.Game.GameStates.Game;
using Runtime.Game.Market;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using Zenject;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private MarketBox[] _marketBoxes;
    
    private ISettingProvider _settingProvider;
    private MarketService _marketService;
    private UserInventoryService _userInventoryService;
    private MarketItemSelectController _marketItemSelectController;
    private ItemDataService _itemDataService;
    private ChickenCareService _chickenCareService;
    private IAudioService _audioService;

    private MarketBox _lastSelectedMarketBox;
    
    [Inject]
    private void Construct(ISettingProvider settingProvider, MarketService marketService, 
        UserInventoryService userInventoryService, MarketItemSelectController marketItemSelectController, 
        ItemDataService itemDataService, ChickenCareService chickenCareService, IAudioService audioService)
    {
        _settingProvider = settingProvider;
        _marketService = marketService;
        _userInventoryService = userInventoryService;
        _marketItemSelectController = marketItemSelectController;
        _itemDataService = itemDataService;
        _chickenCareService = chickenCareService;
        _audioService = audioService;
        
        _marketItemSelectController.OnMarketItemSelect += SetMarketItemOnSale;
    }

    private void OnDestroy()
    {
        _marketItemSelectController.OnMarketItemSelect -= SetMarketItemOnSale;
    }

    private void Awake()
    {
        var config = _settingProvider.Get<MarketPricesConfig>();

        for (int i = 0; i < _marketBoxes.Length; i++)
        {
            var box = _marketBoxes[i];
            box.Initialize(config.BoxesPrices[i], i);
            box.UpdateState(_marketService.GetBoxState(i));

            if (_marketService.GetBoxState(i) == ItemHolderState.Occupied)
            {
                var boxData = _marketService.GetBoxData(i);
                box.SetSellData(_itemDataService.GetItemSprite(boxData.ItemData), boxData.ItemData.Price);
            }
            
            box.OnPurchasePressed += ProcessBoxPurchase;
            box.OnPlaceItemPressed += ProcessItemPlacement;
            box.OnCollectPressed += ProcessCollect;
        }

        StartCoroutine(UpdateTimersCoroutine());
    }

    private IEnumerator UpdateTimersCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < _marketBoxes.Length; i++)
            {
                if(_marketService.GetBoxState(i) != ItemHolderState.Occupied)
                    continue;

                UpdateSellTime(i);
            }

            yield return null;
        }
    }
    
    private void UpdateSellTime(int boxID)
    {
        DateTime endTime = DateTime.FromBinary(Convert.ToInt64(_marketService.GetBoxData(boxID).SellTime));
        TimeSpan elapsed = endTime - DateTime.Now;
        
        if (elapsed.TotalSeconds > 0)
            _marketBoxes[boxID].SetTimeLeft(elapsed.TotalSeconds);
        else
            _marketBoxes[boxID].UpdateState(ItemHolderState.CollectReady);
    }
    
    private void ProcessBoxPurchase(MarketBox box)
    {
        if (!_userInventoryService.CanPurchase(box.SlotPrice))
        {
            _audioService.PlaySound(ConstAudio.ErrorSound);
            return;
        }
        
        _marketService.PurchaseBox(box.SlotID, box.SlotPrice);
        _audioService.PlaySound(ConstAudio.SuccessSound);
        box.UpdateState(ItemHolderState.Purchased);
    }

    private void ProcessItemPlacement(MarketBox box)
    {
        _marketItemSelectController.Run(CancellationToken.None).Forget();
        _lastSelectedMarketBox = box;
    }

    private void SetMarketItemOnSale(ItemData itemData)
    {
        _lastSelectedMarketBox.SetSellData(_itemDataService.GetItemSprite(itemData), itemData.Price);
        _lastSelectedMarketBox.UpdateState(ItemHolderState.Occupied);
        _marketService.PlaceItemForSale(GetBoxId(_lastSelectedMarketBox), itemData, _itemDataService.GetItemSellTime(itemData));
        
        RemoveChickenFromCare(itemData);
        _audioService.PlaySound(ConstAudio.SuccessSound);
    }

    private void RemoveChickenFromCare(ItemData itemData)
    {
        if(itemData.ItemType == ItemType.Egg)
            return;
        
        _chickenCareService.RemoveChickenFromCare(itemData.ItemId);
    }

    private int GetBoxId(MarketBox box)
    {
        for (int i = 0; i < _marketBoxes.Length; i++)
        {
            if(_marketBoxes[i] == box)
                return i;
        }

        return -1;
    }

    private void ProcessCollect(MarketBox box)
    {
        _marketService.CollectCoins(GetBoxId(box));
        box.UpdateState(ItemHolderState.Purchased);
        _audioService.PlaySound(ConstAudio.CoinSound);
        
        AchievementMediator.InvokeSeller();
    }
}
