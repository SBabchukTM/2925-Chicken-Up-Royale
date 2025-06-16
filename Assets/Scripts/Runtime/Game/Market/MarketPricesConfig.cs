using System;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "MarketPricesConfig", menuName = "Config/MarketPricesConfig")]
public class MarketPricesConfig : BaseSettings
{
    public List<int> BoxesPrices = new(){0, 1000, 2000};
    public List<PricesConfig> EggPricesConfig = new();
    public List<PricesConfig> ChickenPricesConfig = new();
    public List<PricesConfig> HenPricesConfig = new();
}

[Serializable]
public class PricesConfig
{
    public int ItemId;
    public int SellPrice;
    public int SecondsToSell;
}
