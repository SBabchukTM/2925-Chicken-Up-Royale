using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Market;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Incubation
{
    public class EggItemSelectFactory : IInitializable
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly UserInventoryService _userInventoryService;
        private readonly ItemDataService _itemDataService;

        private GameObject _prefab;

        public EggItemSelectFactory(IAssetProvider assetProvider, GameObjectFactory gameObjectFactory,
            UserInventoryService userInventoryService, ItemDataService itemDataService)
        {
            _assetProvider = assetProvider;
            _gameObjectFactory = gameObjectFactory;
            _userInventoryService = userInventoryService;
            _itemDataService = itemDataService;
        }

        public async void Initialize()
        {
            _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.ItemSelectPrefab);
        }

        public List<MarketItemSelect> GetItemSelection()
        {
            List<MarketItemSelect> items = new();

            var inv = _userInventoryService.GetInventory();

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

            return items;
        }
    }
}