using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubscriptionData", menuName = "ScriptableObjects/SubscriptionData", order = 1)]
public class SubscriptionData : ScriptableObject
{
    public bool AdsRemove;
    public int MiningDeviceLevelFromBox;
    public int MiningSlots;
    public float SaleInMiningDeviceShop;
}
