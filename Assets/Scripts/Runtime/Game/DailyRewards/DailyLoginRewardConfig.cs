using System;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyLoginRewardConfig", menuName = "Config/DailyLoginRewardConfig")]
public class DailyLoginRewardConfig : BaseSettings   
{
    public List<RewardConfig> Rewards = new ();
    public Sprite LockedRewardSprite;
    public Sprite UnlockedRewardSprite;
    public Sprite LockedClaimButtonSprite;
    public Sprite UnlockedClaimButtonSprite;
}

[Serializable]
public class RewardConfig
{
    public Sprite RewardSprite;
    public int RewardAmount;
}
