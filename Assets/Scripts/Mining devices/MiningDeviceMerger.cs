using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class MiningDeviceMerger : MonoBehaviour
{
    GameRules gameRules;
    P.Player player;
    WindowController windowController;

    private MiningDevice what;
    private MiningDevice with;

    public void Construct(GameRules gameRules, P.Player player, WindowController windowController)
    {
        this.gameRules = gameRules;
        this.player = player;
        this.windowController = windowController;
    }

    internal void TryMerge(MiningDevice what, MiningDevice with)
    {
        this.what = what;
        this.with = with;

        what.GetComponent<MiningDeviceBox>().GetOutOfBox();

        var resultType = FindMergeResult();
        if (resultType != MiningDevices.Null)
        {
            what.UpdateMiningDevice(gameRules.MiningDevices[resultType]);
            what.GetComponent<WhatMergeAnimation>().StartAnimation();

            var thisMiningDeviceLevel = gameRules.MiningDevices[resultType].Level;
            var playerMaxAchivedMiningDeviceLevel = player.MaxAchivedMiningDeviceLevel;

            if (thisMiningDeviceLevel > playerMaxAchivedMiningDeviceLevel)
            {
                MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            }
            else
            {
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            }

            with.GetComponent<WithMergeAnimation>().StartAnimation(() => 
                {
                    if (thisMiningDeviceLevel > playerMaxAchivedMiningDeviceLevel)
                    {
                        player.MaxAchivedMiningDeviceLevel = gameRules.MiningDevices[resultType].Level;
                        windowController.ShowNewMinerWindow();
                        windowController.NewMiningDeviceWindow.ShowNewDeviceWithAnimation(gameRules.MiningDevices[resultType]);
                    }

                    Destroy(with.gameObject);
                });
        }
        else
        {
            throw new Exception();
        }
    }

    public bool CanMerge(MiningDevice miningDevice1, MiningDevice miningDevice2)
    {
        what = miningDevice1;
        with = miningDevice2;

        return FindMergeResult() == MiningDevices.Null ? false : true;
    }

    private MiningDevices FindMergeResult()
    {
        var mergeRules = gameRules.MergeRules.MergeRules;

        for (int i = 0; i < mergeRules.Count; i++)
        {
            if (mergeRules[i].What == what.Data.Type && mergeRules[i].With == with.Data.Type)
            {
                return mergeRules[i].Result;
            }
        }

        return MiningDevices.Null;
    }
}
