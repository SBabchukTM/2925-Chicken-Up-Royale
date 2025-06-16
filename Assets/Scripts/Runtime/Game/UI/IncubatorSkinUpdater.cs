using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class IncubatorSkinUpdater : MonoBehaviour
{
    [SerializeField] private Image _bgImage;

    [Inject]
    private void Construct(ISettingProvider settingProvider, UserInventoryService userInventoryService)
    {
        AreasConfig config = settingProvider.Get<AreasConfig>();
        bool purchased = userInventoryService.GetInventory().PurchasedIncubatorArea;

        if (purchased)
        {
            for (int i = 0; i < config.Areas.Count; i++)
            {
                var area = config.Areas[i];

                if (area.Type == AreaType.Incubator && area.ID != 0)
                {
                    _bgImage.sprite = area.ActualBG;
                    return;
                }
            }
        }
    }
}
