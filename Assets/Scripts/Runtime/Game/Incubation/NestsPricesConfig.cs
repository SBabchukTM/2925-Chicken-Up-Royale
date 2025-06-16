using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

namespace Runtime.Game.Incubation
{
    [CreateAssetMenu(fileName = "NestsPricesConfig", menuName = "Config/NestsPricesConfig")]
    public class NestsPricesConfig : BaseSettings
    {
        public List<int> Prices = new()
        {
            0,
            50,
            100,
            200,
            250,
            500,
            750,
            1000
        };
    }
}