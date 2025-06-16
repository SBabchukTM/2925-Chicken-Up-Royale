using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class ChickensStatusData
    {
        public int ActiveChickenId = 0;
        public List<ChickenStatus> ChickensAtCare = new()
        {
            new ChickenStatus()
            {
                ItemType = ItemType.Chicken,
                Id = 0,
                Happiness = 0.1f,
                Hunger = 0.1f,
                Cleanliness = 0.1f,
                GrowTimeLeft = 60,
                LayTimeLeft = 30,
                TotalLayTime = 30,
                ClaimableEgg = false
            }
        };
    }

    [Serializable]
    public class ChickenStatus
    {
        public ItemType ItemType;
        public int Id;
        public float Happiness;
        public float Hunger;
        public float Cleanliness;
        public int GrowTimeLeft;
        public int LayTimeLeft;
        public int TotalLayTime;
        public bool ClaimableEgg;
    }
}