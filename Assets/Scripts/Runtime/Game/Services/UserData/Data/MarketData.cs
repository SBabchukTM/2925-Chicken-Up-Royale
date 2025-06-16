using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class MarketData
    {
        public List<BoxData> BoxesData = new (){new BoxData()
        {
            State = ItemHolderState.Purchased,
            ItemData = new ItemData()
        }, new BoxData()
        {
            State = ItemHolderState.NotPurchased,
            ItemData = new ItemData()
        }, new BoxData()
        {
            State = ItemHolderState.NotPurchased,
            ItemData = new ItemData()
        }};
    }

    [Serializable]
    public class BoxData
    {
        public ItemHolderState State;
        public ItemData ItemData;
        public string SellTime;
    }

    [Serializable]
    public class ItemData
    {
        public ItemType ItemType;
        public int ItemId;
        public int Price;
    }

    public enum ItemType
    {
        None,
        Egg,
        Chicken,
        Hen
    }
}