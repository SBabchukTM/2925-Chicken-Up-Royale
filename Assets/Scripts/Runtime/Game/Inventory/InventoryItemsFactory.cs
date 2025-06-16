using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Market;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using Zenject;

public class InventoryItemsFactory : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _gameObjectFactory;
    private readonly ItemDataService _itemDataService;
    private readonly UserInventoryService _userInventoryService;
    
    private GameObject _bonusPrefab;
    private GameObject _chickenPrefab;
    private GameObject _eggPrefab;

    public InventoryItemsFactory(IAssetProvider assetProvider, GameObjectFactory gameObjectFactory, 
        ItemDataService itemDataService, UserInventoryService userInventoryService)
    {
        _assetProvider = assetProvider;
        _gameObjectFactory = gameObjectFactory;
        _itemDataService = itemDataService;
        _userInventoryService = userInventoryService;
    }

    public async void Initialize()
    {
        _bonusPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.BonusItemDisplayPrefab);
        _chickenPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.ChickenItemDisplayPrefab);
        _eggPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.EggItemDisplayPrefab);
    }

    public List<BonusItemDisplay> GetBonusItemsDisplay()
    {
        List<BonusItemDisplay> displayList = new List<BonusItemDisplay>();

        var inventory = _userInventoryService.GetInventory();

        var bonusesData = inventory.Boosters;

        for (int i = 0; i < bonusesData.Count; i++)
        {
            var bonusData = bonusesData[i];
            
            var bonusDisplay = _gameObjectFactory.Create<BonusItemDisplay>(_bonusPrefab);
            bonusDisplay.Initialize(_itemDataService.GetBonusSprite(bonusData.ID), bonusData.Amount);
            bonusDisplay.InitializeButton(bonusData.ID);
            displayList.Add(bonusDisplay);
        }
        
        return displayList;
    }
    
    public List<BaseItemDisplay> CreateEggsDisplay()
    {
        List<BaseItemDisplay> displayList = new List<BaseItemDisplay>();
        
        var inventory = _userInventoryService.GetInventory();

        var eggsHeldData = inventory.EggsHeldData;
        for (int i = 0; i < eggsHeldData.Count; i++)
        {
            var itemData = eggsHeldData[i];
            var eggDisplay = _gameObjectFactory.Create<BaseItemDisplay>(_eggPrefab);
            eggDisplay.Initialize(_itemDataService.GetItemSprite(ItemType.Egg, itemData.Id), itemData.Amount);
            displayList.Add(eggDisplay);
        }

        return displayList;
    }
    
    public List<ChickenItemDisplay> CreateChickensDisplay()
    {
        List<ChickenItemDisplay> displayList = new List<ChickenItemDisplay>();
        
        var inventory = _userInventoryService.GetInventory();

        var chickenHeldData = inventory.ChickenHeldData;
        for (int i = 0; i < chickenHeldData.Count; i++)
        {
            var heldData = chickenHeldData[i];
            var chickenDisplay = _gameObjectFactory.Create<ChickenItemDisplay>(_chickenPrefab);
            chickenDisplay.Initialize(_itemDataService.GetItemSprite(ItemType.Chicken, heldData.Id), heldData.Amount);
            
            ItemData itemData = new()
            {
                ItemType = ItemType.Chicken,
                ItemId = heldData.Id,
                Price = _itemDataService.GetItemPrice(ItemType.Chicken, heldData.Id),
            };
            
            chickenDisplay.InitializeButton(itemData);
            
            displayList.Add(chickenDisplay);
        }

        return displayList;
    }
    
        
    public List<ChickenItemDisplay> CreateHenDisplay()
    {
        List<ChickenItemDisplay> displayList = new List<ChickenItemDisplay>();
        
        var inventory = _userInventoryService.GetInventory();

        var henHeldData = inventory.HenHeldData;
        for (int i = 0; i < henHeldData.Count; i++)
        {
            var heldData = henHeldData[i];
            var henDisplay = _gameObjectFactory.Create<ChickenItemDisplay>(_chickenPrefab);
            henDisplay.Initialize(_itemDataService.GetItemSprite(ItemType.Hen, heldData.Id), heldData.Amount);
            
            ItemData itemData = new()
            {
                ItemType = ItemType.Hen,
                ItemId = heldData.Id,
                Price = _itemDataService.GetItemPrice(ItemType.Hen, heldData.Id),
            };
            
            henDisplay.InitializeButton(itemData);
            
            displayList.Add(henDisplay);
        }

        return displayList;
    }
}
