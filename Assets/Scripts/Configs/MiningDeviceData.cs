using UnityEngine;

[CreateAssetMenu(fileName = "MiningDevice", menuName = "ScriptableObjects/MiningDevice", order = 1)]
public class MiningDeviceData : ScriptableObject
{
    public int Level;
    public Sprite Sprite;
    public Sprite SpriteWithOutline;
}