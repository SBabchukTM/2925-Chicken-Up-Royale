using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserInventoryData
    {
        public int Balance = 0;

        public bool PurchasedIncubatorArea;
        public bool PurchasedMarketArea;
        public bool PurchasedCareArea;

        public int UsedBackgroundId = 0;

        public List<int> PurchasedMenuBackgrounds = new() { 0 };
        public List<BoosterData> Boosters = new(){};
        
        public List<ItemHeldData> EggsHeldData = new()
        {
            new ItemHeldData()
            {
                Id = 1,
                Amount = 1
            }
        };

        public List<ItemHeldData> ChickenHeldData = new();
        public List<ItemHeldData> HenHeldData = new();
    }

    [Serializable]
    public class BoosterData
    {
        public int ID;
        public int Amount;
    }

    [Serializable]
    public class ItemHeldData
    {
        public int Id;
        public int Amount;
    }
}