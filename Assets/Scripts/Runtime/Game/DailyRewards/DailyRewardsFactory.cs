using System;
using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.DailyRewards;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;

public class DailyRewardsFactory : IInitializable
{
    private readonly ISettingProvider _settingProvider;
    private readonly IAssetProvider _assetProvider;
    private readonly UserDataService _userDataService;
    private readonly GameObjectFactory _gameObjectFactory;
    private readonly UserLoginService _userLoginService;
    
    private GameObject _dailyRewardsPrefab;

    public DailyRewardsFactory(ISettingProvider settingProvider, IAssetProvider assetProvider,
        UserDataService userDataService, GameObjectFactory gameObjectFactory,
        UserLoginService userLoginService)
    {
        _settingProvider = settingProvider;
        _assetProvider = assetProvider;
        _userDataService = userDataService;
        _gameObjectFactory = gameObjectFactory;
        _userLoginService = userLoginService;
    }
    
    public async void Initialize()
    {
        _dailyRewardsPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.DailyItemPrefab);
    }

    public List<DailyRewardDisplay> CreateDailyRewards()
    {
        List<DailyRewardDisplay> result = new ();
        
        var config = _settingProvider.Get<DailyLoginRewardConfig>();

        var loginStreak = _userLoginService.GetLoginStreak();
        
        MakeCollectedRewardsDisplay(loginStreak, config, result);
        MakeCollectableRewardDisplay(loginStreak, config, result);
        MakeLockedRewardsDisplay(loginStreak, config, result);
        
        return result;
    }

    private void MakeCollectedRewardsDisplay(int loginStreak, DailyLoginRewardConfig config, List<DailyRewardDisplay> result)
    {
        for (int i = 0; i < loginStreak; i++)
        {
            var loginDisplay = _gameObjectFactory.Create<DailyRewardDisplay>(_dailyRewardsPrefab);
            loginDisplay.Initialize(true, false, config.Rewards[i].RewardAmount, i + 1);
            loginDisplay.SetVisuals(config.UnlockedRewardSprite, config.UnlockedClaimButtonSprite, config.Rewards[i].RewardSprite);
            result.Add(loginDisplay);
        }
    }

    private void MakeCollectableRewardDisplay(int loginStreak, DailyLoginRewardConfig config, List<DailyRewardDisplay> result)
    {
        if (_userLoginService.ShowReward(config.Rewards.Count))
        {
            var loginDisplay = _gameObjectFactory.Create<DailyRewardDisplay>(_dailyRewardsPrefab);
            loginDisplay.Initialize( false, true, config.Rewards[loginStreak].RewardAmount, loginStreak + 1);
            loginDisplay.SetVisuals(config.UnlockedRewardSprite, config.UnlockedClaimButtonSprite, config.Rewards[loginStreak].RewardSprite);

            loginDisplay.OnCollected += _userLoginService.UpdateLoginStreak;
            result.Add(loginDisplay);
        }
    }

    private void MakeLockedRewardsDisplay(int loginStreak, DailyLoginRewardConfig config, List<DailyRewardDisplay> result)
    {
        int start = loginStreak + (_userLoginService.ShowReward(config.Rewards.Count) ? 1 : 0);
        
        for (int i = start; i < config.Rewards.Count; i++)
        {
            var loginDisplay = _gameObjectFactory.Create<DailyRewardDisplay>(_dailyRewardsPrefab);
            loginDisplay.Initialize( false, false, config.Rewards[i].RewardAmount, i + 1);
            loginDisplay.SetVisuals(config.LockedRewardSprite, config.LockedClaimButtonSprite, config.Rewards[i].RewardSprite);

            result.Add(loginDisplay);
        }
    }
}
