using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfLocations", menuName = "ScriptableObjects/SetOfLocations", order = 1)]
public class SetOfLocations : ScriptableObject
{
    public List<LocationData> LocationDatas;
}