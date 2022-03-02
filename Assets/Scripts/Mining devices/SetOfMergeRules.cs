using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MergeRule
{
    public MiningDevices What;
    public MiningDevices With;
    public MiningDevices Result;
}

[CreateAssetMenu(fileName = "SetOfMergeRules", menuName = "ScriptableObjects/SetOfMergeRules", order = 1)]
public class SetOfMergeRules : ScriptableObject
{
    public List<MergeRule> MergeRules;
}
