using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Locations
{
    Null = -1,
    Basement = 1,
    Pantry = 2,
    Attic = 3,
    Store = 4,
    Barn = 5,
    Room = 6,
    Garage = 7,
    Office = 8,
    Serverroom = 9,
    Serverfarm = 10

}

[CreateAssetMenu(fileName = "LocationData", menuName = "ScriptableObjects/LocationData", order = 1)]
public class LocationData : ScriptableObject
{
    public string Name;
    public Locations Type;
    public int Level;
    public Sprite Sprite;
    public int MaxLevelOfMiningDevice;
    public int LevelOfMiningDeviceFromBox;

    [Header("Relocate conditions")]
    public int MinMiningDeviceLevelToRelocate;
    public double RelocateCost;

    [Header("Столбцы")]
    public int NumOfColumns;
    [Header("Строки")]
    public int NumOfRows;
    [Header("Майнящие розетки")]
    public int NumOfMiningCells;
}
