using System;
using System.Collections;
using System.Collections.Generic;
using MergeMiner.Core.State.Data;
using UnityEngine;

public enum Boosts
{
    Null,
    MiningPower,
    BoxFillingSpeed,
    EverySlotIsMining,
    TimeRewind,
    MinerFromBoxUpgrade,
    Diamonds
}

public class BoostController : MonoBehaviour, ISavable
{
    private MiningController miningController;
    private Box box;
    private MergeFieldBuilder mergeFieldBuilder;
    private P.Player player;

    private List<BoostTimeLeft> boostsTimeLeft;

    public void Construct(MiningController miningController, Box box, MergeFieldBuilder mergeFieldBuilder, P.Player player)
    {
        this.miningController = miningController;
        this.box = box;
        this.mergeFieldBuilder = mergeFieldBuilder;
        this.player = player;

        boostsTimeLeft = new List<BoostTimeLeft>();

        Load();
    }

    private void Update()
    {
        DecreaseBoostLeftTimeForFrame();
        RemoveExpiredBoosts();
    }

    private void RemoveExpiredBoosts()
    {
        var boostsToRemove = boostsTimeLeft.FindAll(boostTimeLeft => boostTimeLeft.TimeLeft <= 0);

        foreach (var boostToRemove in boostsToRemove)
        {
            RemoveBoost(boostToRemove.Type);
        }

        boostsTimeLeft.RemoveAll(boostTimeLeft => boostTimeLeft.TimeLeft <= 0);
    }
    private void DecreaseBoostLeftTimeForFrame()
    {
        foreach (var boostTimeLeft in boostsTimeLeft)
        {
            boostTimeLeft.DecreaseTimeLeft(Time.deltaTime);
        }
    }

    public void Boost(BoostType type, float power = 1, float duration = 0)
    {
        switch (type)
        {
            case BoostType.PowerUp:
                BoostMiningPower(power);
                boostsTimeLeft.Add(new BoostTimeLeft(type, power, duration));
                break;
            case BoostType.PowerAll:
                BoostSlots();
                boostsTimeLeft.Add(new BoostTimeLeft(type, power, duration));
                break;
            case BoostType.BoxFilling:
                BoostBoxFillingSpeed(power);
                boostsTimeLeft.Add(new BoostTimeLeft(type, power, duration));
                break;
            case BoostType.TimeRewind:
                RewindTime(duration);
                break;
            case BoostType.Miners:
                UpgradeMinerFromBox();
                break;
            case BoostType.Gems:
                player.AddDiamonds(power);
                break;
        }
    }
    public void Boost(BoostType type, int power = 1, int duration = 0)
    {
        Boost(type, (float)power, (float)duration);
    }
    public void RemoveBoost(BoostType type)
    {
        switch (type)
        {
            case BoostType.PowerUp:
                RemoveMiningPowerBoost();
                break;
            case BoostType.PowerAll:
                RemoveSlotsBoost();
                break;
            case BoostType.BoxFilling:
                RemoveBoxFillingSpeedBoost();
                break;
        }
    }

    private void BoostMiningPower(float boostNum)
    {
        miningController.Boost = boostNum;
    }
    private void RemoveMiningPowerBoost()
    {
        miningController.Boost = 1;
    }

    private void BoostBoxFillingSpeed(float boostNum)
    {
        box.Boost = boostNum;
    }
    private void RemoveBoxFillingSpeedBoost()
    {
        box.Boost = 1;
    }

    private void BoostSlots()
    {
        foreach (var cell in mergeFieldBuilder.Cells)
        {
            if (!cell.IsMiningCell)
            {
                cell.BecomeMiningCell();
            }
        }
    }
    private void RemoveSlotsBoost()
    {
        for (int i = 0; i < mergeFieldBuilder.Cells.Count; i++)
        {
            if (i >= mergeFieldBuilder.CurLocSettings.NumOfMiningCells)
            {
                mergeFieldBuilder.Cells[i].BecomeNotMiningCell();
            }
        }
    }

    private void RewindTime(float timeInSeconds)
    {
        miningController.AddCoinsForPeriod(timeInSeconds);
    }

    private void UpgradeMinerFromBox()
    {
        player.UpgradeMiningDeviceFromBox();
    }

    public void Save()
    {
        Saver.Save(SaveNames.BoostsTimeLeft, new BoostTimeLeftSave(boostsTimeLeft));
    }
    public void Load()
    {
        var defaultSave = new BoostTimeLeftSave(new List<BoostTimeLeft>());
        var boosts = Saver.Load(SaveNames.BoostsTimeLeft, defaultSave).BoostsTimeLeft;

        foreach (var boost in boosts)
        {
            Boost(boost.Type, boost.Power, boost.TimeLeft);
        }
    }
}

[Serializable]
public class BoostTimeLeft
{
    [SerializeField]
    private BoostType type;
    [SerializeField]
    private float power;
    [SerializeField]
    private float timeLeft;

    public BoostType Type
    {
        get => type;
    }
    public float Power
    {
        get => power;
    }
    public float TimeLeft
    {
        get => timeLeft;
    }

    public BoostTimeLeft(BoostType type, float power, float timeLeft)
    {
        this.type = type;
        this.timeLeft = timeLeft;
        this.power = power;
    }
    public float DecreaseTimeLeft(float time)
    {
        timeLeft -= time;
        return timeLeft;
    }
}

[Serializable]
public class BoostTimeLeftSave
{
    [SerializeField]
    public List<BoostTimeLeft> BoostsTimeLeft;

    public BoostTimeLeftSave(List<BoostTimeLeft> boostsTimeLeft)
    {
        BoostsTimeLeft = boostsTimeLeft;
    }
}