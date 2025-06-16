using System;
using System.Collections.Generic;
using Runtime.Game.Care;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

namespace Runtime.Game.EggLaying
{
    public class EggLayingService
    {
        private const int ChickenTypes = 6;
        
        private readonly UserDataService _userDataService;
        private readonly UserInventoryService _userInventoryService;
        private readonly ChickenCareService _chickenCareService;

        private float _timer;
        
        public EggLayingService(UserDataService userDataService, UserInventoryService userInventoryService,
            ChickenCareService chickenCareService)
        {
            _userDataService = userDataService;
            _userInventoryService = userInventoryService;
            _chickenCareService = chickenCareService;
        }

        public void UpdateEggTimeOnline(float deltaTime)
        {
            var activeChicken = _chickenCareService.GetActiveChickenStatus();
            _timer += deltaTime;

            if (_timer > 1)
            {
                _timer = 0;

                if (activeChicken.LayTimeLeft <= 0)
                {
                    activeChicken.ClaimableEgg = true;
                    return;
                }
                
                activeChicken.LayTimeLeft--;
            }
        }
        
        public void UpdateEggTimeOffline()
        {
            var activeChicken = _chickenCareService.GetActiveChickenStatus();
            
            if(activeChicken.ItemType != ItemType.Hen)
                return;
            
            string lastVisitTimeStr = _userDataService.GetUserData().UserLoginData.LastChickenVisitTimeString;
            
            if(lastVisitTimeStr == String.Empty)
                return;
            
            DateTime now = DateTime.Now;
            DateTime lastVisit = Convert.ToDateTime(lastVisitTimeStr);
            
            int secondsSinceLastVisit = (int)now.Subtract(lastVisit).TotalSeconds;

            activeChicken.LayTimeLeft -= secondsSinceLastVisit;
            if (activeChicken.LayTimeLeft <= 0)
                activeChicken.ClaimableEgg = true;
        }
        
        public int SelectLayEggTime()
        {
            var eggsHeld = _userInventoryService.GetInventory().EggsHeldData;
            var chickenHeld = _userInventoryService.GetInventory().ChickenHeldData;
            var henHeld = _userInventoryService.GetInventory().HenHeldData;
            
            int chickenTypes = eggsHeld.Count + chickenHeld.Count + henHeld.Count;
            
            const float baseTime = 30f;
            const float maxTime = 86400f;
            const float growthRate = 4.0f;
            
            float time = baseTime * Mathf.Pow(growthRate, chickenTypes);
            return Mathf.Clamp(Mathf.RoundToInt(time), (int)baseTime, (int)maxTime);
        }
        
        public void AddRandomEgg()
        {
            var eggsHeld = _userInventoryService.GetInventory().EggsHeldData;
            var chickenHeld = _userInventoryService.GetInventory().ChickenHeldData;
            var henHeld = _userInventoryService.GetInventory().HenHeldData;
            
            HashSet<int> idHashSet = new ();

            foreach (var egg in eggsHeld)
                idHashSet.Add(egg.Id);

            foreach (var chink in chickenHeld)
                idHashSet.Add(chink.Id);

            foreach (var hen in henHeld)
                idHashSet.Add(hen.Id);

            for (int i = 0; i < ChickenTypes; i++)
            {
                if (idHashSet.Add(i))
                {
                    _userInventoryService.AddEgg(i);
                    return;
                }
            }
        }
    }
}