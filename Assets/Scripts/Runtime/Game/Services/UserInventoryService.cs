using System;
using System.Collections.Generic;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;

namespace Runtime.Game.Services
{
    public class UserInventoryService
    {
        private readonly UserDataService _userDataService;
        
        public event Action<int> OnBalanceChanged;

        public UserInventoryService(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public UserInventoryData GetInventory() => _userDataService.GetUserData().UserInventoryData;

        public int GetBalance() => GetInventory().Balance;
        
        public void AddBalance(int amount)
        {
            var inventoryData = _userDataService.GetUserData().UserInventoryData;
            
            int balance = inventoryData.Balance;
            balance += amount;
            
            inventoryData.Balance = balance;
            OnBalanceChanged?.Invoke(balance);
        }
        
        public bool CanPurchase(int price) => _userDataService.GetUserData().UserInventoryData.Balance >= price;

        public void PurchaseBackground(int id, int price)
        {
            var inventoryData = GetInventory();
            inventoryData.PurchasedMenuBackgrounds.Add(id);
            UpdateUsedBackground(id);
            AddBalance(-price);
        }

        public void AddChicken(int id)
        {
            var inventoryData = GetInventory();
            var chickens = inventoryData.ChickenHeldData;

            for (int i = 0; i < chickens.Count; i++)
            {
                var chickenEntry = chickens[i];
                if (chickenEntry.Id == id)
                {
                    chickenEntry.Amount++;
                    return;
                }
            }
            
            chickens.Add(new ItemHeldData()
            {
                Id = id,
                Amount = 1,
            });
        }

        public void AddEgg(int id)
        {
            var inventoryData = GetInventory();
            inventoryData.EggsHeldData.Add(new()
            {
                Id = id,
                Amount = 1,
            });
        }
        
        public void RemoveItem(ItemData itemData) => RemoveItem(itemData.ItemType, itemData.ItemId);
        
        public void RemoveItem(ItemType itemType, int id)
        {
            var inventoryData = GetInventory();

            List<ItemHeldData> heldDataList = null;

            switch (itemType)
            {
                case ItemType.Hen:
                    heldDataList = inventoryData.HenHeldData;
                    break;
                case ItemType.Chicken:
                    heldDataList = inventoryData.ChickenHeldData;
                    break;
                case ItemType.Egg:
                    heldDataList = inventoryData.EggsHeldData;
                    break;
            }
            
            if(heldDataList == null)
                return;

            for (int i = 0; i < heldDataList.Count; i++)
            {
                var heldData = heldDataList[i];

                if(heldData.Id != id)
                    continue;
                
                heldData.Amount--;
                if(heldData.Amount == 0)
                    heldDataList.RemoveAt(i);
                break;
            }
        }
        
        public void UpdateUsedBackground(int id) => GetInventory().UsedBackgroundId = id;

        public bool IsBackgroundPurchased(int id) => GetInventory().PurchasedMenuBackgrounds.Contains(id);
        
        public void PurchaseArea(AreaType type, int price)
        {
            switch (type)
            {
                case AreaType.Care:
                    GetInventory().PurchasedCareArea = true;
                    break;
                case AreaType.Incubator:
                    GetInventory().PurchasedIncubatorArea = true;
                    break;
                case AreaType.Market:
                    GetInventory().PurchasedMarketArea = true;
                    break;
            }
            
            AddBalance(-price);
        }
        
        public void AddBooster(int id, int price)
        {
            var inventoryData = _userDataService.GetUserData().UserInventoryData;
            
            AddBalance(-price);
            var boosters = inventoryData.Boosters;
            for (int i = 0; i < boosters.Count; i++)
            {
                var boosterData = boosters[i];

                if (boosterData.ID == id)
                {
                    boosterData.Amount++;
                    return;
                }
            }
            
            boosters.Add(new ()
            {
                ID = id,
                Amount = 1,
            });
        }
        
        public void RemoveBooster(int id)
        {
            var inventoryData = _userDataService.GetUserData().UserInventoryData;
            
            var boosters = inventoryData.Boosters;
            for (int i = 0; i < boosters.Count; i++)
            {
                var boosterData = boosters[i];

                if (boosterData.ID == id)
                {
                    boosterData.Amount--;

                    if (boosterData.Amount == 0)
                        boosters.Remove(boosterData);
                    
                    return;
                }
            }
        }
    }
}