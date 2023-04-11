using System.Collections.Generic;
using MergeMiner.Core.State.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfFlyingBonuses", menuName = "ScriptableObjects/SetOfFlyingBonuses", order = 1)]
public class SetOfFlyingBonuses : ScriptableObject
{
    public FlyingBonusData this[BonusType type]
    {
        get => FlyingBonuses.Find(data => data.Type == type);
    }

    public List<FlyingBonusData> FlyingBonuses;
}
