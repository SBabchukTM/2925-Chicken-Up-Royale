using System;
using UnityEngine.Serialization;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserLoginData
    {
        public string LastChickenVisitTimeString = String.Empty;
        public string LastIncubatorVisitTimeString = String.Empty;
        public string LastDailyRewardLoginTimeString = String.Empty;
        public int LoginStreak = 0;
    }
}