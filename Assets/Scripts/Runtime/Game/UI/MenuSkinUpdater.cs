using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuSkinUpdater : MonoBehaviour
{
    [SerializeField] private Image _bgImage;

    [Inject]
    private void Construct(ISettingProvider settingProvider, UserInventoryService userInventoryService)
    {
        BackgroundsConfig config = settingProvider.Get<BackgroundsConfig>();
        int usedId = userInventoryService.GetInventory().UsedBackgroundId;

        for (int i = 0; i < config.Backgrounds.Count; i++)
        {
            var background = config.Backgrounds[i];

            if (background.ID == usedId)
            {
                _bgImage.sprite = background.ActualBG;
                return;
            }
        }
    }
}
