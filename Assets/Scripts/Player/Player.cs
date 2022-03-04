using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceTypes
{
    Coins,
    Diamonds,
    Ads
}

[Serializable]
public class Player : ISavable
{
    public event Action<double> NumOfCoinsUpdated;
    public event Action MaxAchivedMiningDeviceLevelUpdated;

    [SerializeField]
    private double coins;
    [SerializeField]
    private float diamonds;
    [SerializeField]
    private int miningDeviceFromBoxUpgradeLevel;
    [SerializeField]
    private Locations curLocation;
    [SerializeField]
    private int maxAchivedMiningDeviceLevel;

    public double Coins
    {
        get => coins;
        private set
        {
            coins = value;
            NumOfCoinsUpdated?.Invoke(coins);
        }
    }
    public float Diamonds
    {
        get => diamonds;
        set
        {
            diamonds = value;
        }
    }
    public double GetNumOfResource(ResourceTypes type)
    {
        switch (type)
        {
            case ResourceTypes.Coins:
                return Coins;
            case ResourceTypes.Diamonds:
                return Diamonds;
            default:
                return -1;
        };
    }


    public Locations CurLocation
    {
        get => curLocation;
        private set
        {
            curLocation = value;
        }
    }
    public int MiningDeviceFromBoxUpgradeLevel
    {
        get => miningDeviceFromBoxUpgradeLevel;
    }
    public int MaxAchivedMiningDeviceLevel
    {
        get => maxAchivedMiningDeviceLevel;
        set
        {
            maxAchivedMiningDeviceLevel = value;
            MaxAchivedMiningDeviceLevelUpdated?.Invoke();
        }
    }

    public Player(float coins, float diamonds, int miningDeviceFromBoxUpgradeLevel, Locations curLocation, int maxAchivedMiningDeviceLevel)
    {
        Coins = coins;
        Diamonds = diamonds;
        this.miningDeviceFromBoxUpgradeLevel = miningDeviceFromBoxUpgradeLevel;
        this.curLocation = curLocation;
        this.maxAchivedMiningDeviceLevel = maxAchivedMiningDeviceLevel;
    }

    public static Player GetStartPlayerSettings()
    {
        return new Player(0, 10, 0, Locations.Basement, 1);
    }

    public void SpendDiamonds(float diamonds)
    {
        Diamonds -= diamonds;
    }

    public void AddCoins(double coins)
    {
        Coins += coins;
    }
    public void SpendCoins(double coins)
    {
        Coins -= coins;
    }
    public void AddDiamonds(float diamonds)
    {
        Diamonds += diamonds;
    }
    public void UpgradeMiningDeviceFromBox()
    {
        miningDeviceFromBoxUpgradeLevel++;
    }
    public void UpdateLocation(Locations type)
    {
        curLocation = type;
    }

    public void Save()
    {
        Saver.Save(SaveNames.Player, this);
    }
}
