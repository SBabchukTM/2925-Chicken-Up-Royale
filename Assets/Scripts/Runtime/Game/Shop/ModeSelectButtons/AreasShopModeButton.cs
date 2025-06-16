using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using UnityEngine;

public class AreasShopModeButton : ShopModeButton
{
    private async void Start()
    {
        AreasConfig config = SettingProvider.Get<AreasConfig>();

        GameObject prefab = await AssetProvider.Load<GameObject>(ConstPrefabs.AreaShopItemPrefab);
        
        foreach (var areaItem in config.Areas)
        {
            if(areaItem.ID == 0)
                continue;
            
            var display = GameObjectFactory.Create<AreaItemButton>(prefab);
            display.SetData(areaItem.Sprite, areaItem.Price);
            display.Initialize(IsItemPurchased(areaItem), areaItem.Type);
            display.transform.SetParent(Content, false);

            display.OnPurchased += ProcessPurchase;
        }
    }

    private bool IsItemPurchased(AreaConfig area)
    {
        switch (area.Type)
        {
            case AreaType.Care:
                return UserInventoryService.GetInventory().PurchasedCareArea;
            case AreaType.Incubator:
                return UserInventoryService.GetInventory().PurchasedIncubatorArea;
            case AreaType.Market:
                return UserInventoryService.GetInventory().PurchasedMarketArea;
        }

        return false;
    }

    private void ProcessPurchase(ShopItemDisplay item)
    {
        if (UserInventoryService.CanPurchase(item.Price))
        {
            AreaItemButton areaItemButton = (AreaItemButton)item;
            UserInventoryService.PurchaseArea(areaItemButton.AreaType, areaItemButton.Price);
            areaItemButton.Initialize(true, areaItemButton.AreaType);
            AudioService.PlaySound(ConstAudio.SuccessSound);
            AchievementMediator.InvokeStylist();
        }
        else
            AudioService.PlaySound(ConstAudio.ErrorSound);
    }
}
