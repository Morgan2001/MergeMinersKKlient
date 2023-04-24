using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfMiningDeviceDatas", menuName = "ScriptableObjects/SetOfMiningDeviceDatas", order = 1)]
public class SetOfMiningDeviceDatas : ScriptableObject
{
    public MiningDeviceData this[int level]
    {
        get => MiningDeviceDatas.Find(data => data.Level == level);
    }

    public List<MiningDeviceData> MiningDeviceDatas;
}