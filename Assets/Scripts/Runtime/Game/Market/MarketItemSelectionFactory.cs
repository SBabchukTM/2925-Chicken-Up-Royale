using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Care;
using Runtime.Game.Market;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using Zenject;

public class MarketItemSelectionFactory : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _gameObjectFactory;
    private readonly UserInventoryService _userInventoryService;
    private readonly ItemDataService _itemDataService;
    private readonly ChickenCareService _chickenCareService;

    private GameObject _prefab;

    public MarketItemSelectionFactory(IAssetProvider assetProvider, GameObjectFactory gameObjectFactory,
         UserInventoryService userInventoryService, ItemDataService itemDataService, ChickenCareService chickenCareService)
    {
        _assetProvider = assetProvider;
        _gameObjectFactory = gameObjectFactory;
        _userInventoryService = userInventoryService;
        _itemDataService = itemDataService;
        _chickenCareService = chickenCareService;
    }
    
    public async void Initialize()
    {
        _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.ItemSelectPrefab);
    }

    public List<MarketItemSelect> GetItemSelection()
    {
        List<MarketItemSelect> items = new ();

        var inv = _userInventoryService.GetInventory();

        AddHens(inv, items);
        AddChickens(inv, items);
        AddEggs(inv, items);
        
        return items;
    }

    private void AddHens(UserInventoryData inv, List<MarketItemSelect> items)
    {
        foreach (var hen in inv.HenHeldData)
        {
            int id = hen.Id;
            ItemType itemType = ItemType.Hen;
            
            if(id == _chickenCareService.GetIdOfActiveChicken())
                continue;
            
            var display = _gameObjectFactory.Create<MarketItemSelect>(_prefab);
   
            ItemData itemData = new()
            {
                ItemType = itemType,
                ItemId = id,
                Price = _itemDataService.GetItemPrice(itemType, id)
            };
            display.Initialize(itemData, _itemDataService.GetItemSprite(itemType, id) ,hen.Amount);
            
            items.Add(display);
        }
    }
    
    private void AddChickens(UserInventoryData inv, List<MarketItemSelect> items)
    {
        foreach (var chicken in inv.ChickenHeldData)
        {
            int id = chicken.Id;
            ItemType itemType = ItemType.Chicken;
            
            if(id == _chickenCareService.GetIdOfActiveChicken())
                continue;
         
            var display = _gameObjectFactory.Create<MarketItemSelect>(_prefab);

            ItemData itemData = new()
            {
                ItemType = itemType,
                ItemId = id,
                Price = _itemDataService.GetItemPrice(itemType, id)
            };
            display.Initialize(itemData, _itemDataService.GetItemSprite(itemType, id), chicken.Amount);
            items.Add(display);
        }
    }
    
    private void AddEggs(UserInventoryData inv, List<MarketItemSelect> items)
    {
        foreach (var egg in inv.EggsHeldData)
        {
            var display = _gameObjectFactory.Create<MarketItemSelect>(_prefab);

            int id = egg.Id;
            ItemType itemType = ItemType.Egg;
            
            ItemData itemData = new()
            {
                ItemType = itemType,
                ItemId = id,
                Price = _itemDataService.GetItemPrice(itemType, id)
            };
            display.Initialize(itemData, _itemDataService.GetItemSprite(itemType, id), egg.Amount);
            items.Add(display);
        }
    }
}
