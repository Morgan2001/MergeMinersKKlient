using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelRewards", menuName = "ScriptableObjects/SetOfWheelRewards", order = 1)]
public class SetOfWheelRewards : ScriptableObject
{
    public List<WheelReward> Rewards;
}