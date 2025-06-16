using System.Linq;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

namespace Runtime.Game.Market
{
    public class ItemDataService
    {
        private readonly ISettingProvider _settingProvider;
        
        private MarketPricesConfig _marketPricesConfig;
        private GameItemsConfig _gameItemsConfig;
        private EggIncubationConfig _eggIncubationConfig;
        private BoosterItemsConfig _boosterItemsConfig;
        private ChickenGrowConfig _chickenGrowConfig;

        public ItemDataService(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        public int GetItemPrice(ItemData itemData) => GetItemPrice(itemData.ItemType, itemData.ItemId);

        public int GetItemSellTime(ItemData itemData)
        {
            switch (itemData.ItemType)
            {
                case ItemType.Egg:
                    return GetEggSellTime(itemData.ItemId);
                case ItemType.Chicken:
                    return GetChickenSellTime(itemData.ItemId);
                case ItemType.Hen:
                    return GetHenSellTime(itemData.ItemId);
            }

            return 0;
        }

        public int GetEggHatchTime(int itemId)
        {
            if (!_eggIncubationConfig)
                _eggIncubationConfig = _settingProvider.Get<EggIncubationConfig>();
            
            return _eggIncubationConfig.IncubationTimeSeconds[itemId];
        }

        public Sprite GetItemSprite(ItemData itemData) => GetItemSprite(itemData.ItemType, itemData.ItemId);
        
        public int GetItemPrice(ItemType type, int id)
        {
            if(!_marketPricesConfig)
                _marketPricesConfig = _settingProvider.Get<MarketPricesConfig>();
            
            switch (type)
            {
                case ItemType.Hen:
                    return GetHenPrice(id);
                case ItemType.Egg:
                    return GetEggPrice(id);
                case ItemType.Chicken:
                    return GetChickenPrice(id);
            }
            
            return 0;
        }

        public Sprite GetBonusSprite(int id)
        {
            if(_boosterItemsConfig == null)
                _boosterItemsConfig = _settingProvider.Get<BoosterItemsConfig>();
            
            if(id < 0 || id > _boosterItemsConfig.BoosterItems.Count)
                return null;
            
            return _boosterItemsConfig.BoosterItems[id].Sprite;
        }

        public int GetChickGrowTime(int id)
        {
            if(_chickenGrowConfig == null)
                _chickenGrowConfig = _settingProvider.Get<ChickenGrowConfig>();
            
            return _chickenGrowConfig.TimeToGrowSeconds[id];
        }
        
        public Sprite GetItemSprite(ItemType type, int id)
        {
            if(!_gameItemsConfig)
                _gameItemsConfig = _settingProvider.Get<GameItemsConfig>();
            
            switch (type)
            {
                case ItemType.Hen:
                    return GetHenSprite(id);
                case ItemType.Egg:
                    return GetEggSprite(id);
                case ItemType.Chicken:
                    return GetChickenSprite(id);
            }
            
            return null;
        }
        
        private int GetHenPrice(int id) => _marketPricesConfig.HenPricesConfig.First(x => x.ItemId == id).SellPrice;

        private int GetChickenPrice(int id) => _marketPricesConfig.ChickenPricesConfig.First(x => x.ItemId == id).SellPrice;

        private int GetEggPrice(int id) => _marketPricesConfig.EggPricesConfig.First(x => x.ItemId == id).SellPrice;

        private Sprite GetHenSprite(int id) => _gameItemsConfig.HenItemDisplayConfigs.First(x => x.ItemId == id).ItemSprite;

        private Sprite GetChickenSprite(int id) => _gameItemsConfig.ChickenItemDisplayConfigs.First(x => x.ItemId == id).ItemSprite;

        private Sprite GetEggSprite(int id) => _gameItemsConfig.EggItemDisplayConfigs.First(x => x.ItemId == id).ItemSprite;

        private int GetEggSellTime(int id) =>
            _marketPricesConfig.EggPricesConfig.Find(x => x.ItemId == id).SecondsToSell;
        
        private int GetChickenSellTime(int id) =>
            _marketPricesConfig.ChickenPricesConfig.Find(x => x.ItemId == id).SecondsToSell;
        
        private int GetHenSellTime(int id) =>
            _marketPricesConfig.HenPricesConfig.Find(x => x.ItemId == id).SecondsToSell;
    }
}