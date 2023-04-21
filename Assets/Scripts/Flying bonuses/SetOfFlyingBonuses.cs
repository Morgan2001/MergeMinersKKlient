using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfFlyingBonuses", menuName = "ScriptableObjects/SetOfFlyingBonuses", order = 1)]
public class SetOfFlyingBonuses : ScriptableObject
{
    public FlyingBonusData this[string id] => FlyingBonuses.Find(data => data.Id == id);

    public List<FlyingBonusData> FlyingBonuses;
}
