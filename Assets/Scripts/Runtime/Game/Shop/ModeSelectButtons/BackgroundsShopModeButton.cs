using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using UnityEngine;

public class BackgroundsShopModeButton : ShopModeButton
{
    private List<BackGroundItemButton> _buttons;
    
    private async void Start()
    {
        BackgroundsConfig config = SettingProvider.Get<BackgroundsConfig>();

        GameObject prefab = await AssetProvider.Load<GameObject>(ConstPrefabs.BackgroundShopItemPrefab);

        _buttons = new();
        
        foreach (var bg in config.Backgrounds)
        {
            if(bg.ID == 0)
                continue;
            
            var display = GameObjectFactory.Create<BackGroundItemButton>(prefab);
            display.SetData(bg.Sprite, bg.Price);
            display.Initialize(bg.ID, IsItemPurchased(bg));
            display.SetSelected(IsSelected(bg));
            display.transform.SetParent(Content, false);

            display.OnPurchased += ProcessPurchase;
            
            _buttons.Add(display);
        }
    }

    private bool IsItemPurchased(BackgroundConfig bg)
    {
        return UserInventoryService.GetInventory().PurchasedMenuBackgrounds.Contains(bg.ID);
    }

    private bool IsSelected(BackgroundConfig bg)
    {
        return UserInventoryService.GetInventory().UsedBackgroundId == bg.ID;
    }

    private void ProcessPurchase(ShopItemDisplay item)
    {
        BackGroundItemButton bgItemButton = (BackGroundItemButton)item;

        if (UserInventoryService.IsBackgroundPurchased(bgItemButton.Id))
        {
            AudioService.PlaySound(ConstAudio.SuccessSound);
            UserInventoryService.UpdateUsedBackground(bgItemButton.Id);
        }
        else if (UserInventoryService.CanPurchase(item.Price))
        {
            AudioService.PlaySound(ConstAudio.SuccessSound);
            UserInventoryService.PurchaseBackground(bgItemButton.Id, bgItemButton.Price);
            bgItemButton.Initialize(bgItemButton.Id, true);
            AchievementMediator.InvokeNewEnvironment();
        }
        else
            AudioService.PlaySound(ConstAudio.ErrorSound);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetSelected(UserInventoryService.GetInventory().UsedBackgroundId == _buttons[i].Id);
        }
    }
}
