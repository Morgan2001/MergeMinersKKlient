using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MiningDeviceReward
{
    public int maxAchivedMiningDeviceLevel;
    public double walletReward;
}

[CreateAssetMenu(menuName = "ScriptableObjects/WalletRewards")]
public class WalletRewards : ScriptableObject
{
    public List<MiningDeviceReward> rewards;

    public double GetRewardByLevel(int level)
    {
        return rewards.Find(reward => reward.maxAchivedMiningDeviceLevel == level).walletReward;
    }
}
