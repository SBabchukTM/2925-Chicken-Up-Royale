using System;
using Runtime.Game.Care;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

namespace Runtime.Game.Incubation
{
    public class IncubatorService
    {
        private const int MaxNests = 8;
        private readonly UserDataService _userDataService;
        private readonly UserInventoryService _userInventoryService;
        private readonly BoostersService _boostersService;

        public IncubatorService(UserDataService userDataService, UserInventoryService userInventoryService,
            BoostersService boostersService)
        {
            _userDataService = userDataService;
            _userInventoryService = userInventoryService;
            _boostersService = boostersService;
        }

        private IncubatorData GetIncubatorData() => _userDataService.GetUserData().IncubatorData;
        
        public ItemHolderState GetNestState(int nestID)
        {
            if (!IsIdValid(nestID))
                return ItemHolderState.NotPurchased;

            var box = GetIncubatorData().Nests[nestID];
            return box.State;
        }

        public NestData GetNestData(int nestId)
        {
            if (!IsIdValid(nestId))
                return null;

            var nest = GetIncubatorData().Nests[nestId];
            return nest;
        }

        public void PlaceEggToHatch(int nestId, ItemData itemData, int sellTime)
        {
            if (!IsIdValid(nestId))
                return;
            
            var nestData = GetIncubatorData().Nests[nestId];
            nestData.EggId = itemData.ItemId;
            nestData.State = ItemHolderState.Occupied;
            nestData.EggHatchTimeLeft = sellTime;
            
            _userInventoryService.RemoveItem(itemData);
        }

        public void PurchaseNest(int nestId, int price)
        {
            if (!IsIdValid(nestId))
                return;
            
            GetIncubatorData().Nests[nestId].State = ItemHolderState.Purchased;
            _userInventoryService.AddBalance(-price);
        }

        public void UpdateHatchTimeOffline()
        {
            string lastVisitTimeStr = _userDataService.GetUserData().UserLoginData.LastIncubatorVisitTimeString;
            
            if(lastVisitTimeStr == String.Empty)
                return;
            
            var incubatorData = _userDataService.GetUserData().IncubatorData;
            
            DateTime now = DateTime.Now;
            DateTime lastVisit = Convert.ToDateTime(lastVisitTimeStr);
            
            int secondsSinceLastVisit = (int)now.Subtract(lastVisit).TotalSeconds;
            int boosterTime = GetBoosterDurationTime(now, lastVisit, _userDataService.GetUserData().BoostersData.IncubateBoosterEndTime);
            
            boosterTime = Mathf.Clamp(boosterTime, 0, secondsSinceLastVisit);
            
            for (int i = 0; i < incubatorData.Nests.Count; i++)
            {
                var nestData = incubatorData.Nests[i];
                
                if(nestData.State != ItemHolderState.Occupied)
                    continue;
                
                nestData.EggHatchTimeLeft -= secondsSinceLastVisit + boosterTime;
            }
        }

        public void UpdateHatchTimeOnline(int nestId, float deltaTime)
        {
            GetNestData(nestId).EggHatchTimeLeft -= deltaTime * GetIncubationBoost();
        }
        
        public void HatchEgg(int nestId)
        {
            if(!IsIdValid(nestId))
                return;

            var nestData = GetNestData(nestId);
            nestData.State = ItemHolderState.Purchased;
            _userInventoryService.AddChicken(nestData.EggId);
            AchievementMediator.InvokeFirstHatch();
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
        
        private float GetIncubationBoost() => _boostersService.IsBoosterActive(BoosterTypes.Incubate) ? 2 : 1;

        private bool IsIdValid(int id) => id is >= 0 and < MaxNests;
    }
}