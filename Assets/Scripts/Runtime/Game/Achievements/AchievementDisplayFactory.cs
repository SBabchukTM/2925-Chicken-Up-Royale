using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

public class AchievementDisplayFactory : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _gameObjectFactory;
    private readonly UserDataService _userDataService;

    private GameObject _prefab;

    public AchievementDisplayFactory(IAssetProvider assetProvider, GameObjectFactory gameObjectFactory,
        UserDataService userDataService)
    {
        _assetProvider = assetProvider;
        _gameObjectFactory = gameObjectFactory;
        _userDataService = userDataService;
    }
    
    public async void Initialize()
    {
        _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.AchievementDisplayPrefab);
    }

    public List<AchievementDisplay> CreateAchievementDisplayList()
    {
        List<AchievementDisplay> achievementDisplayList = new List<AchievementDisplay>();
        
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().FirstHatch));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().NewCaretaker));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().BathTime));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().SnackTime));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().PlayTime));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().BoosterShopper));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().Cheater));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().Stylist));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().GrowTime));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().Seller));
        achievementDisplayList.Add(CreateAchievementDisplay(GetData().NewEnvironment));
        
        return achievementDisplayList;
    }

    private UserAchievementsData GetData() => _userDataService.GetUserData().UserAchievementsData;
    
    private AchievementDisplay CreateAchievementDisplay(AchievementData achievement)
    {
        AchievementDisplay display = _gameObjectFactory.Create<AchievementDisplay>(_prefab);
        display.Initialize(achievement);
        return display;
    }
}
