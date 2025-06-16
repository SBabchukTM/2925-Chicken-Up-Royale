using System;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using UnityEngine;

public class BoostersShopModeButton : ShopModeButton
{
    private async void Start()
    {
        BoosterItemsConfig config = SettingProvider.Get<BoosterItemsConfig>();

        GameObject prefab = await AssetProvider.Load<GameObject>(ConstPrefabs.BoosterShopItemPrefab);
        
        foreach (var boosterItem in config.BoosterItems)
        {
            var display = GameObjectFactory.Create<BoosterItemButton>(prefab);
            display.SetData(boosterItem.Sprite, boosterItem.Price);
            display.Initialize(boosterItem.Id);
            display.transform.SetParent(Content, false);
            
            display.OnPurchased += ProcessPurchase;
        }
    }

    private void ProcessPurchase(ShopItemDisplay item)
    {
        if (UserInventoryService.CanPurchase(item.Price))
        {
            UserInventoryService.AddBooster(((BoosterItemButton)item).Id, item.Price);
            AudioService.PlaySound(ConstAudio.SuccessSound);
            AchievementMediator.InvokeBoosterShopper();
        }
        else
        {
            AudioService.PlaySound(ConstAudio.ErrorSound);
        }
    }
}
