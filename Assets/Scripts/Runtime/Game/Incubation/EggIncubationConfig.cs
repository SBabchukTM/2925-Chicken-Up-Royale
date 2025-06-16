using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "EggIncubationConfig", menuName = "Config/EggIncubationConfig")]
public class EggIncubationConfig : BaseSettings
{
    public List<int> IncubationTimeSeconds = new List<int>();
}
