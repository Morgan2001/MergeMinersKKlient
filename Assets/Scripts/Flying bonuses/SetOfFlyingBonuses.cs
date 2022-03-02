using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfFlyingBonuses", menuName = "ScriptableObjects/SetOfFlyingBonuses", order = 1)]
public class SetOfFlyingBonuses : ScriptableObject
{
    public FlyingBonusData this[FlyingBonuses type]
    {
        get => FlyingBonuses.Find(data => data.Type == type);
    }

    public List<FlyingBonusData> FlyingBonuses;
}
