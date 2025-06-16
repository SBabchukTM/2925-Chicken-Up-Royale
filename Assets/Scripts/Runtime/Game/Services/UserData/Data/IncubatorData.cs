using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class IncubatorData
    {
        public List<NestData> Nests = new List<NestData>()
        {
            new NestData() { State = ItemHolderState.Purchased },
            new NestData() { State = ItemHolderState.NotPurchased, },
            new NestData() { State = ItemHolderState.NotPurchased, },
            new NestData() { State = ItemHolderState.NotPurchased, },
            new NestData() { State = ItemHolderState.NotPurchased, },
            new NestData() { State = ItemHolderState.NotPurchased, },
            new NestData() { State = ItemHolderState.NotPurchased, },
            new NestData() { State = ItemHolderState.NotPurchased, },
        };
    }

    [Serializable]
    public class NestData
    {
        public ItemHolderState State;
        public int EggId;
        public float EggHatchTimeLeft;
    }
}