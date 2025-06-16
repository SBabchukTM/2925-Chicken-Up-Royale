using System;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Services.UserData
{
    public class MarketService
    {
        private const int MaxBoxes = 3;
        private readonly UserDataService _userDataService;
        private readonly UserInventoryService _userInventoryService;

        public MarketService(UserDataService userDataService, UserInventoryService userInventoryService)
        {
            _userDataService = userDataService;
            _userInventoryService = userInventoryService;
        }

        private MarketData GetMarketData() => _userDataService.GetUserData().MarketData;
        
        public ItemHolderState GetBoxState(int boxID)
        {
            if (!IsIdValid(boxID))
                return ItemHolderState.NotPurchased;

            var box = GetMarketData().BoxesData[boxID];
            return box.State;
        }

        public BoxData GetBoxData(int boxID)
        {
            if (!IsIdValid(boxID))
                return null;

            var box = GetMarketData().BoxesData[boxID];
            return box;
        }

        public void PlaceItemForSale(int boxId, ItemData itemData, int sellTime)
        {
            if (!IsIdValid(boxId))
                return;
            
            var boxData = GetMarketData().BoxesData[boxId];
            boxData.ItemData.ItemId = itemData.ItemId;
            boxData.ItemData.Price = itemData.Price;
            boxData.ItemData.ItemType = itemData.ItemType;
            boxData.State = ItemHolderState.Occupied;
            boxData.SellTime = DateTime.Now.AddSeconds(sellTime).ToBinary().ToString();
            
            _userInventoryService.RemoveItem(itemData);
        }

        public void PurchaseBox(int boxID, int price)
        {
            if (!IsIdValid(boxID))
                return;
            
            GetMarketData().BoxesData[boxID].State = ItemHolderState.Purchased;
            _userInventoryService.AddBalance(-price);
        }

        public void CollectCoins(int boxID)
        {
            if (!IsIdValid(boxID))
                return;
            
            var boxData = GetMarketData().BoxesData[boxID];
            _userInventoryService.AddBalance(boxData.ItemData.Price);
            boxData.State = ItemHolderState.Purchased;
        }
        
        private bool IsIdValid(int id) => id is >= 0 and < MaxBoxes;
    }
}