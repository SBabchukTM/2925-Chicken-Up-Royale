using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterItemsConfig", menuName = "Config/BoosterItemsConfig")]
public class BoosterItemsConfig : BaseSettings
{
    public List<BoosterItemData> BoosterItems;
}

[Serializable]
public class BoosterItemData
{
    public int Id;
    public Sprite Sprite;
    public int Price;
}
