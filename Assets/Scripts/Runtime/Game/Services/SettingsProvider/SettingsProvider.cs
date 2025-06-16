using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Incubation;
using Runtime.Game.Services.ScreenOrientation;

namespace Runtime.Game.Services.SettingsProvider
{
    public class SettingsProvider : ISettingProvider
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<Type, BaseSettings> _settings = new Dictionary<Type, BaseSettings>();

        public SettingsProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var screenOrientationConfig = await _assetProvider.Load<ScreenOrientationConfig>(ConstConfigs.ScreenOrientationConfig);
            var audioConfig = await _assetProvider.Load<AudioConfig>(ConstConfigs.AudioConfig);
            var dailyLoginRewardConfig = await _assetProvider.Load<DailyLoginRewardConfig>(ConstConfigs.DailyLoginRewardConfig);
            var boostersConfig = await _assetProvider.Load<BoosterItemsConfig>(ConstConfigs.BoosterItemsConfig);
            var areasConfig = await _assetProvider.Load<AreasConfig>(ConstConfigs.AreasItemsConfig);
            var backgroundsConfig = await _assetProvider.Load<BackgroundsConfig>(ConstConfigs.BackgroundsItemsConfig);
            var marketPricesConfig = await _assetProvider.Load<MarketPricesConfig>(ConstConfigs.MarketPricesConfig);
            var gameItemsConfig = await _assetProvider.Load<GameItemsConfig>(ConstConfigs.GameItemsConfig);
            var nestPricesConfig = await _assetProvider.Load<NestsPricesConfig>(ConstConfigs.NestsPricesConfig);
            var eggHatchTimeConfig = await _assetProvider.Load<EggIncubationConfig>(ConstConfigs.EggIncubationConfig);
            var chickenGrowTimeConfig = await _assetProvider.Load<ChickenGrowConfig>(ConstConfigs.ChickenGrowConfig);

            Set(screenOrientationConfig);
            Set(audioConfig);
            Set(dailyLoginRewardConfig);
            Set(boostersConfig);
            Set(areasConfig);
            Set(backgroundsConfig);
            Set(marketPricesConfig);
            Set(gameItemsConfig);
            Set(nestPricesConfig);
            Set(eggHatchTimeConfig);
            Set(chickenGrowTimeConfig);
        }

        public T Get<T>() where T : BaseSettings
        {
            if (_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];
                return setting as T;
            }

            throw new Exception("No setting found");
        }

        public void Set(BaseSettings config)
        {
            if (_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}