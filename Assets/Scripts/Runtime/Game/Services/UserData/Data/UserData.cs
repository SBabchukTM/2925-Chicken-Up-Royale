using System;
using System.Collections.Generic;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.UserAccountSystem;

namespace Runtime.Game.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public UserAccountData UserAccountData = new UserAccountData();
        public UserLoginData UserLoginData = new UserLoginData();
        public UserInventoryData UserInventoryData = new UserInventoryData();
        public UserDoodleClearTime UserDoodleClearTime = new UserDoodleClearTime();
        public MarketData MarketData = new MarketData();
        public IncubatorData IncubatorData = new IncubatorData();
        public ChickensStatusData ChickensStatusData = new ChickensStatusData();
        public BoostersData BoostersData = new BoostersData();
        public UserAchievementsData UserAchievementsData = new UserAchievementsData();
    }
}