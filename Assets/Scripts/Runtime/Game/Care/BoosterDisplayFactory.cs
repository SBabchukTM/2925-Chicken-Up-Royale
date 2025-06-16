using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Market;
using Runtime.Game.Services;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Care
{
    public class BoosterDisplayFactory : IInitializable
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly BoostersService _boostersService;
        private readonly ItemDataService _itemDataService;

        private GameObject _prefab;
        
        public BoosterDisplayFactory(IAssetProvider assetProvider, GameObjectFactory gameObjectFactory,
            ItemDataService itemDataService, BoostersService boostersService)
        {
            _assetProvider = assetProvider;
            _gameObjectFactory = gameObjectFactory;
            _itemDataService = itemDataService;
            _boostersService = boostersService;
        }
        
        public async void Initialize()
        {
            _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.BoosterDisplayPrefab);
        }

        public List<BoosterDisplay> GetBoosterDisplay()
        {
            List<BoosterDisplay> boosterDisplay = new List<BoosterDisplay>();

            if (_boostersService.IsBoosterActive(BoosterTypes.Grow))
            {
                var display = _gameObjectFactory.Create<BoosterDisplay>(_prefab);
                display.Initialize(_itemDataService.GetBonusSprite(0));
                boosterDisplay.Add(display);
            }
            
            if (_boostersService.IsBoosterActive(BoosterTypes.Happiness))
            {
                var display = _gameObjectFactory.Create<BoosterDisplay>(_prefab);
                display.Initialize(_itemDataService.GetBonusSprite(1));
                boosterDisplay.Add(display);
            }
            
            if (_boostersService.IsBoosterActive(BoosterTypes.Cleanliness))
            {
                var display = _gameObjectFactory.Create<BoosterDisplay>(_prefab);
                display.Initialize(_itemDataService.GetBonusSprite(2));
                boosterDisplay.Add(display);
            }
            
            if (_boostersService.IsBoosterActive(BoosterTypes.Hunger))
            {
                var display = _gameObjectFactory.Create<BoosterDisplay>(_prefab);
                display.Initialize(_itemDataService.GetBonusSprite(3));
                boosterDisplay.Add(display);
            }
            
            if (_boostersService.IsBoosterActive(BoosterTypes.Incubate))
            {
                var display = _gameObjectFactory.Create<BoosterDisplay>(_prefab);
                display.Initialize(_itemDataService.GetBonusSprite(4));
                boosterDisplay.Add(display);
            }
            
            return boosterDisplay;
        }
    }
}