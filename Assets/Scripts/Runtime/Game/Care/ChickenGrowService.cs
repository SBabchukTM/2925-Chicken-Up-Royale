using System;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

namespace Runtime.Game.Care
{
    public class ChickenGrowService
    {
        private const float MinStatusThreshold = 0.3f;
        private const float GrowBoost = 2;

        private const float HappinessDecreaseSpeed = 0.001f;
        private const float HungerDecreaseSpeed = 0.002f;
        private const float CleanlinessDecreaseSpeed = 0.0015f;

        private readonly UserDataService _userDataService;
        private readonly ChickenCareService _chickenCareService;
        private readonly BoostersService _boostersService;
        private readonly UserInventoryService _userInventoryService;

        private float _timeAccumulator;

        public ChickenGrowService(UserDataService userDataService, ChickenCareService chickenCareService,
            BoostersService boostersService, UserInventoryService userInventoryService)
        {
            _userDataService = userDataService;
            _chickenCareService = chickenCareService;
            _boostersService = boostersService;
            _userInventoryService = userInventoryService;
        }

        public bool UpdateOnlineGrowTime(float deltaTime)
        {
            var activeChicken = _chickenCareService.GetActiveChickenStatus();
            if (activeChicken == null)
                return false;

            UpdateOnlineStatuses(deltaTime, activeChicken);

            if (!CanGrow(activeChicken))
                return false;

            _timeAccumulator += deltaTime * GetGrowBonus();

            while (_timeAccumulator >= 1f)
            {
                activeChicken.GrowTimeLeft -= 1;
                _timeAccumulator -= 1f;
            }

            if (activeChicken.GrowTimeLeft <= 0)
            {
                GrowChickenToHen(activeChicken);
                _timeAccumulator = 0f;
            }

            return true;
        }

        private void GrowChickenToHen(ChickenStatus chickenStatus)
        {
            chickenStatus.ItemType = ItemType.Hen;

            var chickens = _userInventoryService.GetInventory().ChickenHeldData;
            var hens = _userInventoryService.GetInventory().HenHeldData;
            
            for (int i = 0; i < chickens.Count; i++)
            {
                if (chickens[i].Id == chickenStatus.Id)
                {
                    chickens.RemoveAt(i);
                    break;
                }
            }
            
            hens.Add(new()
            {
                Id = chickenStatus.Id,
                Amount = 1,
            });
            
            AchievementMediator.InvokeGrowTime();
        }
        
        private float GetGrowBonus() => _boostersService.IsBoosterActive(BoosterTypes.Grow) ? GrowBoost : 1;

        private void UpdateOnlineStatuses(float deltaTime, ChickenStatus activeChicken)
        {
            if (_boostersService.IsBoosterActive(BoosterTypes.Happiness))
                activeChicken.Happiness = 1;
            else
                activeChicken.Happiness -= HappinessDecreaseSpeed * deltaTime;

            if (_boostersService.IsBoosterActive(BoosterTypes.Hunger))
                activeChicken.Hunger = 1;
            else
                activeChicken.Hunger -= HungerDecreaseSpeed * deltaTime;

            if (_boostersService.IsBoosterActive(BoosterTypes.Cleanliness))
                activeChicken.Cleanliness = 1;
            else
                activeChicken.Cleanliness -= CleanlinessDecreaseSpeed * deltaTime;
        }

        public void UpdateOfflineGrowTime()
        {
            var totalSecondsPassed = GetSecondsSinceLastLogin();
            var activeChicken = _chickenCareService.GetActiveChickenStatus();

            if (totalSecondsPassed == 0 || activeChicken == null)
                return;

            var now = DateTime.Now;
            var lastLogin = Convert.ToDateTime(_userDataService.GetUserData().UserLoginData.LastChickenVisitTimeString);

            float happinessBoosterTime = GetBoosterDurationTime(now, lastLogin,
                _userDataService.GetUserData().BoostersData.HappinessBoosterEndTime);
            float hungerBoosterTime = GetBoosterDurationTime(now, lastLogin,
                _userDataService.GetUserData().BoostersData.HungerBoosterEndTime);
            float cleanlinessBoosterTime = GetBoosterDurationTime(now, lastLogin,
                _userDataService.GetUserData().BoostersData.CleanlinessBoosterEndTime);

            var happinessDecayTime = Mathf.Max(0, totalSecondsPassed - happinessBoosterTime);
            var hungerDecayTime = Mathf.Max(0, totalSecondsPassed - hungerBoosterTime);
            var cleanlinessDecayTime = Mathf.Max(0, totalSecondsPassed - cleanlinessBoosterTime);

            activeChicken.Happiness =
                Mathf.Clamp01(activeChicken.Happiness - happinessDecayTime * HappinessDecreaseSpeed);
            activeChicken.Hunger = Mathf.Clamp01(activeChicken.Hunger - hungerDecayTime * HungerDecreaseSpeed);
            activeChicken.Cleanliness =
                Mathf.Clamp01(activeChicken.Cleanliness - cleanlinessDecayTime * CleanlinessDecreaseSpeed);
            
            if (activeChicken.ItemType != ItemType.Chicken)
                return;
            
            var happinessLimitTime =
                Mathf.Clamp01(activeChicken.Happiness - MinStatusThreshold) / HappinessDecreaseSpeed;
            var hungerLimitTime = Mathf.Clamp01(activeChicken.Hunger - MinStatusThreshold) / HungerDecreaseSpeed;
            var cleanlinessLimitTime =
                Mathf.Clamp01(activeChicken.Cleanliness - MinStatusThreshold) / CleanlinessDecreaseSpeed;

            var effectiveDecayTime = Mathf.Min(happinessDecayTime, hungerDecayTime, cleanlinessDecayTime);
            var maxGrowableTime =
                Mathf.Min(happinessLimitTime, hungerLimitTime, cleanlinessLimitTime, effectiveDecayTime);
            var growableSeconds = Mathf.FloorToInt(maxGrowableTime);

            var growBoosterTime = GetBoosterDurationTime(now, lastLogin,
                _userDataService.GetUserData().BoostersData.GrowBoosterEndTime);
            
            growBoosterTime = Mathf.Clamp(growBoosterTime, 0, growableSeconds);
            
            activeChicken.GrowTimeLeft -= growableSeconds + growBoosterTime;
            
            if (activeChicken.GrowTimeLeft <= 0)
            {
                GrowChickenToHen(activeChicken);
            }
        }

        private int GetBoosterDurationTime(DateTime now, DateTime lastVisit, string boosterEndString)
        {
            if (string.IsNullOrWhiteSpace(boosterEndString))
                return 0;

            if (!DateTime.TryParse(boosterEndString, out var boosterEnd))
                return 0;

            if (boosterEnd <= lastVisit)
                return 0;

            if (boosterEnd >= now)
                return (int)(now - lastVisit).TotalSeconds;

            return (int)(boosterEnd - lastVisit).TotalSeconds;
        }

        private int GetSecondsSinceLastLogin()
        {
            var lastVisitString = _userDataService.GetUserData().UserLoginData.LastChickenVisitTimeString;

            if (lastVisitString == string.Empty)
                return 0;

            var lastTime = Convert.ToDateTime(lastVisitString);
            return (int)(DateTime.Now - lastTime).TotalSeconds;
        }

        private bool CanGrow(ChickenStatus chickenStatus)
        {
            return chickenStatus.Happiness > MinStatusThreshold &&
                   chickenStatus.Hunger > MinStatusThreshold &&
                   chickenStatus.Cleanliness > MinStatusThreshold;
        }
    }
}