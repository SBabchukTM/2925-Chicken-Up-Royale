using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "GameItemsConfig", menuName = "Config/GameItemsConfig")]
public class GameItemsConfig : BaseSettings
{
    public List<ItemDisplayConfig> EggItemDisplayConfigs = new List<ItemDisplayConfig>();
    public List<ItemDisplayConfig> ChickenItemDisplayConfigs = new List<ItemDisplayConfig>();
    public List<ItemDisplayConfig> HenItemDisplayConfigs = new List<ItemDisplayConfig>();
}

[Serializable]
public class ItemDisplayConfig
{
    public Sprite ItemSprite;
    public int ItemId;
}