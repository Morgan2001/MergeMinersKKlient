using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfLocations", menuName = "ScriptableObjects/SetOfLocations", order = 1)]
public class SetOfLocations : ScriptableObject
{
    public LocationData this[Locations type]
    {
        get => LocationDatas.Find(data => data.Type == type);
    }

    public List<LocationData> LocationDatas;
}
