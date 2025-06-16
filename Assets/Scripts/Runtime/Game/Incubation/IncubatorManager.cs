using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.DailyRewards;
using Runtime.Game.Incubation;
using Runtime.Game.Market;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using Zenject;

public class IncubatorManager : MonoBehaviour
{
    [SerializeField] private IncubatorNest[] _incubatorNests;
    
    private ISettingProvider _settingProvider;
    private IncubatorService _incubatorService;
    private UserInventoryService _userInventoryService;
    private EggItemSelectController _eggItemSelectController;
    private ItemDataService _itemDataService;
    private UserLoginService _userLoginService;
    private IAudioService _audioService;

    private IncubatorNest _lastSelectedNest;
    
    [Inject]
    private void Construct(ISettingProvider settingProvider, IncubatorService incubatorService, 
        UserInventoryService userInventoryService, EggItemSelectController eggItemSelectController, 
        ItemDataService itemDataService, UserLoginService userLoginService, IAudioService audioService)
    {
        _settingProvider = settingProvider;
        _incubatorService = incubatorService;
        _userInventoryService = userInventoryService;
        _eggItemSelectController = eggItemSelectController;
        _itemDataService = itemDataService;
        _userLoginService = userLoginService;
        _audioService = audioService;
        
        _eggItemSelectController.OnEggSelect += SetEggToHatch;
    }

    private void OnDestroy()
    {
        _eggItemSelectController.OnEggSelect -= SetEggToHatch;
    }

    private void Awake()
    {
        var config = _settingProvider.Get<NestsPricesConfig>();

        for (int i = 0; i < _incubatorNests.Length; i++)
        {
            var nest = _incubatorNests[i];
            nest.Initialize(config.Prices[i], i);
            nest.UpdateState(_incubatorService.GetNestState(i));

            if (_incubatorService.GetNestState(i) == ItemHolderState.Occupied)
            {
                var nestData = _incubatorService.GetNestData(i);
                nest.SetVisuals(_itemDataService.GetItemSprite(ItemType.Egg, nestData.EggId));
            }
            
            nest.OnPurchasePressed += ProcessNestPurchase;
            nest.OnPlaceItemPressed += ProcessEggPlacement;
            nest.OnClaimPressed += ProcessEggClaim;
        }

        _incubatorService.UpdateHatchTimeOffline();
        StartCoroutine(UpdateTimersCoroutine());
        _userLoginService.RecordIncubatorVisitTime();
    }
    
    private IEnumerator UpdateTimersCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < _incubatorNests.Length; i++)
            {
                if(_incubatorService.GetNestState(i) != ItemHolderState.Occupied)
                    continue;

                UpdateHatchTime(i);
            }

            yield return null;
        }
    }
    
    private void UpdateHatchTime(int boxID)
    {
        _incubatorService.UpdateHatchTimeOnline(boxID, Time.deltaTime);
        float timeLeft = _incubatorService.GetNestData(boxID).EggHatchTimeLeft;
        
        if (timeLeft > 0)
            _incubatorNests[boxID].UpdateTimeLeft(timeLeft);
        else
            _incubatorNests[boxID].UpdateState(ItemHolderState.CollectReady);
    }

    private void ProcessNestPurchase(IncubatorNest nest)
    {
        if (!_userInventoryService.CanPurchase(nest.SlotPrice))
        {
            _audioService.PlaySound(ConstAudio.ErrorSound);
            return;
        }
        
        _incubatorService.PurchaseNest(nest.SlotID, nest.SlotPrice);
        nest.UpdateState(ItemHolderState.Purchased);
        _audioService.PlaySound(ConstAudio.SuccessSound);
    }

    private void ProcessEggPlacement(IncubatorNest box)
    {
        _eggItemSelectController.Run(CancellationToken.None).Forget();
        _lastSelectedNest = box;
    }

    private void SetEggToHatch(ItemData itemData)
    {
        _lastSelectedNest.SetVisuals(_itemDataService.GetItemSprite(ItemType.Egg, itemData.ItemId));
        _lastSelectedNest.UpdateState(ItemHolderState.Occupied);
        
        _incubatorService.PlaceEggToHatch(GetNestId(_lastSelectedNest), itemData, _itemDataService.GetEggHatchTime(itemData.ItemId));
        _audioService.PlaySound(ConstAudio.SuccessSound);
    }

    private int GetNestId(IncubatorNest nest)
    {
        for (int i = 0; i < _incubatorNests.Length; i++)
        {
            if(_incubatorNests[i] == nest)
                return i;
        }

        return -1;
    }

    private void ProcessEggClaim(IncubatorNest nest)
    {
        nest.UpdateState(ItemHolderState.Purchased);
        _incubatorService.HatchEgg(GetNestId(nest));
        _audioService.PlaySound(ConstAudio.ChickenSound);
    }
}
