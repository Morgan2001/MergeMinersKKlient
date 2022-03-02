using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Locations
{
    Null = -1,
    Room = 1,
    Garage = 2,
    Office = 3,
    ServerRoom = 4
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
