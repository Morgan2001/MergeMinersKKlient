using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDeviceChooser
{
    Player player;
    GameRules gameRules;
    Subscription subscription;

    public MiningDeviceChooser(Player player, GameRules gameRules, Subscription subscription)
    {
        this.player = player;
        this.gameRules = gameRules;
        this.subscription = subscription;
    }

    public MiningDevices ChooseRandomMiningDeviceFromBoxToMaxAchived()
    {
        var fromLevel = GetMiningDeviceFromBox().Level;
        var toLevel = player.MaxAchivedMiningDeviceLevel;

        var level = UnityEngine.Random.Range(fromLevel, toLevel + 1);

        return gameRules.MiningDevices.MiningDeviceDatas.Find(device => device.Level == level).Type;
    }

    public MiningDeviceData GetMiningDeviceFromBox()
    {
        var fromSub = 0;

        if (subscription.IsActive())
        {
            fromSub = subscription.Data.MiningDeviceLevelFromBox;
        }

        return gameRules.MiningDevices[gameRules.Locations[player.CurLocation].Level + player.MiningDeviceFromBoxUpgradeLevel + fromSub];
    }
}
