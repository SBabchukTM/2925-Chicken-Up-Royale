using System;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "AreasConfig", menuName = "Config/AreasConfig")]
public class AreasConfig : BaseSettings
{
    public List<AreaConfig> Areas = new List<AreaConfig>();
}

[Serializable]
public class AreaConfig
{
    public int ID;
    public AreaType Type;
    public Sprite Sprite;
    public Sprite ActualBG;
    public int Price;
}

public enum AreaType
{
    Incubator,
    Market,
    Care
}
