using System.Collections;
using System.Collections.Generic;
using P;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningDeviceOffer", menuName = "ScriptableObjects/MiningDeviceOffer", order = 1)]
public class MiningDeviceOffer : ScriptableObject
{
    public int Id;
    public int MaxAchivedMinerLevelRequired;
    public int MiningDeviceLevel;
    public double StartPrice;
    public ResourceTypes Currency;
    public double PriceIncreaseAfterPurchase;
}
