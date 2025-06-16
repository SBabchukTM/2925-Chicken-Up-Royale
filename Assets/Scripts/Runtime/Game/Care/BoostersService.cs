using System;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Care
{
    public class BoostersService
    {
        private readonly UserDataService _userDataService;
        private readonly UserInventoryService _userInventoryService;

        public BoostersService(UserDataService userDataService, UserInventoryService userInventoryService)
        {
            _userDataService = userDataService;
            _userInventoryService = userInventoryService;
        }

        public bool IsBoosterActive(BoosterTypes type)
        {
            DateTime now = DateTime.Now;
            
            var boosterData = GetBoosterData();
            switch (type)
            {
                case BoosterTypes.Grow:
                    var growTime = boosterData.GrowBoosterEndTime;
                    if (growTime == String.Empty)
                        return false;
                    return (Convert.ToDateTime(growTime) - now).TotalSeconds > 0;
                
                case BoosterTypes.Happiness:
                    var happyTime = boosterData.HappinessBoosterEndTime;
                    if (happyTime == String.Empty)
                        return false;
                    return (Convert.ToDateTime(happyTime) - now).TotalSeconds > 0;
                
                case BoosterTypes.Cleanliness:
                    var cleanTime = boosterData.CleanlinessBoosterEndTime;
                    if (cleanTime == String.Empty)
                        return false;
                    return (Convert.ToDateTime(cleanTime) - now).TotalSeconds > 0;
                
                case BoosterTypes.Hunger:
                    var hungerTime = boosterData.HungerBoosterEndTime;
                    if (hungerTime == String.Empty)
                        return false;
                    return (Convert.ToDateTime(hungerTime) - now).TotalSeconds > 0;
                
                case BoosterTypes.Incubate:
                    var incTime = boosterData.IncubateBoosterEndTime;
                    if (incTime == String.Empty)
                        return false;
                    return (Convert.ToDateTime(incTime) - now).TotalSeconds > 0;
            }

            return false;
        }
        
        public void ApplyBooster(BoosterTypes type)
        {
            var boosterData = GetBoosterData();
            
            AchievementMediator.InvokeCheater();
            switch (type)
            {
                case BoosterTypes.Grow:
                    var growTime = boosterData.GrowBoosterEndTime;
                    if (growTime == String.Empty)
                        boosterData.GrowBoosterEndTime = DateTime.Now.AddHours(24).ToString();
                    else
                    {
                        var time = Convert.ToDateTime(growTime);
                        boosterData.GrowBoosterEndTime = time.AddHours(24).ToString();
                    }
                    _userInventoryService.RemoveBooster(0);
                    break;
                
                case BoosterTypes.Happiness:
                    var happyTime = boosterData.HappinessBoosterEndTime;
                    if (happyTime == String.Empty)
                        boosterData.HappinessBoosterEndTime = DateTime.Now.AddHours(24).ToString();
                    else
                    {
                        var time = Convert.ToDateTime(happyTime);
                        boosterData.HappinessBoosterEndTime = time.AddHours(24).ToString();
                    }
                    _userInventoryService.RemoveBooster(1);
                    break;
                
                case BoosterTypes.Cleanliness:
                    var cleanTime = boosterData.CleanlinessBoosterEndTime;
                    if (cleanTime == String.Empty)
                        boosterData.CleanlinessBoosterEndTime = DateTime.Now.AddHours(24).ToString();
                    else
                    {
                        var time = Convert.ToDateTime(cleanTime);
                        boosterData.CleanlinessBoosterEndTime = time.AddHours(24).ToString();
                    }
                    _userInventoryService.RemoveBooster(2);
                    break;
                
                case BoosterTypes.Hunger:
                    var hungerTime = boosterData.HungerBoosterEndTime;
                    if (hungerTime == String.Empty)
                        boosterData.HungerBoosterEndTime = DateTime.Now.AddHours(24).ToString();
                    else
                    {
                        var time = Convert.ToDateTime(hungerTime);
                        boosterData.HungerBoosterEndTime = time.AddHours(24).ToString();
                    }
                    _userInventoryService.RemoveBooster(3);
                    break;
                
                case BoosterTypes.Incubate:
                    var incTime = boosterData.IncubateBoosterEndTime;
                    if (incTime == String.Empty)
                        boosterData.IncubateBoosterEndTime = DateTime.Now.AddHours(24).ToString();
                    else
                    {
                        var time = Convert.ToDateTime(incTime);
                        boosterData.IncubateBoosterEndTime = time.AddHours(24).ToString();
                    }
                    _userInventoryService.RemoveBooster(4);
                    break;
            }
        }

        private BoostersData GetBoosterData() => _userDataService.GetUserData().BoostersData;
    }

    public enum BoosterTypes
    {
        Happiness,
        Hunger,
        Cleanliness,
        Incubate,
        Grow
    }
}