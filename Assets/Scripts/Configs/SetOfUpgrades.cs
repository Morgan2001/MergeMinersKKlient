using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrades", menuName = "ScriptableObjects/SetOfUpgrades", order = 1)]
public class SetOfUpgrades : ScriptableObject
{
    public List<UpgradeOffer> Upgrades;
}