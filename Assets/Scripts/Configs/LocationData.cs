using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "ScriptableObjects/LocationData", order = 1)]
public class LocationData : ScriptableObject
{
    public string Name;
    public int Level;
    public Sprite Sprite;
}