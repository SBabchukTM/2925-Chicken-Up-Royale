using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "ChickenGrowConfig", menuName = "Config/ChickenGrowConfig")]
public class ChickenGrowConfig : BaseSettings
{
    public List<int> TimeToGrowSeconds = new List<int>();
}
