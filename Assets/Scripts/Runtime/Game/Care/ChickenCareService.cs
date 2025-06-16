using Runtime.Game.Market;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

namespace Runtime.Game.Care
{
    public class ChickenCareService
    {
        private readonly UserDataService _userDataService;
        private readonly ItemDataService _itemDataService;

        public ChickenCareService(UserDataService userDataService, ItemDataService itemDataService)
        {
            _userDataService = userDataService;
            _itemDataService = itemDataService;
        }

        public void AddHappiness(float amount)
        {
            var chickenStatus = GetActiveChickenStatus();
            if(chickenStatus == null)
                return;
            
            float current = chickenStatus.Happiness;
            current = Mathf.Clamp01(current + amount);
            chickenStatus.Happiness = current;
        }

        public void AddHunger(float amount)
        {
            var chickenStatus = GetActiveChickenStatus();
            if(chickenStatus == null)
                return;
            
            float current = chickenStatus.Hunger;
            current = Mathf.Clamp01(current + amount);
            chickenStatus.Hunger = current;
        }

        public int GetActiveChickenArrayId() => GetChickensStatusData().ActiveChickenId;

        public int GetIdOfActiveChicken()
        {
            int arrayId = GetActiveChickenArrayId();
            var chickensAtCare = GetChickensStatusData().ChickensAtCare;
            return chickensAtCare[arrayId].Id;
        }
        
        public void AddCleanliness(float amount)
        {
            var chickenStatus = GetActiveChickenStatus();
            if(chickenStatus == null)
                return;
            
            float current = chickenStatus.Cleanliness;
            current = Mathf.Clamp01(current + amount);
            chickenStatus.Cleanliness = current;
        }

        public ChickenStatus GetActiveChickenStatus()
        {
            int activeChickenId = GetActiveChickenArrayId();
            if (activeChickenId < 0 || activeChickenId > GetChickensStatusData().ChickensAtCare.Count)
                return null;
            
            return GetChickensStatusData().ChickensAtCare[activeChickenId];
        }

        public void RemoveChickenFromCare(int chickenId)
        {
            var chickensAtCare = GetChickensStatusData().ChickensAtCare;

            var activeChicken = chickensAtCare[GetActiveChickenArrayId()];

            for (int i = 0; i < chickensAtCare.Count; i++)
            {
                var chick = chickensAtCare[i];
                if (chick.Id == chickenId)
                {
                    chickensAtCare.RemoveAt(i);
                    GetChickensStatusData().ActiveChickenId = chickensAtCare.FindIndex(x => x == activeChicken);
                    return;
                }
            }
        }

        public void AddChickenToCare(ItemType type, int chickenId)
        {
            var chickensAtCare = GetChickensStatusData().ChickensAtCare;
            
            for (int i = 0; i < chickensAtCare.Count; i++)
            {
                var chick = chickensAtCare[i];
                if (chick.Id == chickenId)
                {
                    GetChickensStatusData().ActiveChickenId = i;
                    return;
                }
            }
            
            chickensAtCare.Add(new ChickenStatus()
            {
                ItemType = type,
                Id = chickenId,
                Happiness = Random.Range(0, 1f),
                Cleanliness = Random.Range(0, 1f),
                Hunger = Random.Range(0, 1f),
                GrowTimeLeft = _itemDataService.GetChickGrowTime(chickenId)
            });
            GetChickensStatusData().ActiveChickenId = chickensAtCare.Count - 1;
        }
        
        private ChickensStatusData GetChickensStatusData() => _userDataService.GetUserData().ChickensStatusData;
    }
}