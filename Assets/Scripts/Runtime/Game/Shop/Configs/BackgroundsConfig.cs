using System;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundsConfig", menuName = "Config/BackgroundsConfig")]
public class BackgroundsConfig : BaseSettings
{
    public List<BackgroundConfig> Backgrounds = new List<BackgroundConfig>();
}

[Serializable]
public class BackgroundConfig
{
    public int ID;
    public Sprite Sprite;
    public Sprite ActualBG;
    public int Price;
}
