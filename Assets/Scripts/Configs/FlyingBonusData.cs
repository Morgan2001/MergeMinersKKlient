using UnityEngine;

[CreateAssetMenu(fileName = "FlyingBonusData", menuName = "ScriptableObjects/FlyingBonusData", order = 1)]
public class FlyingBonusData : ScriptableObject
{
    public string Id;
    public Sprite Sprite;
    public int NumOfBouncesToDestroy;
    public float TimeToFLyBetweenSides;
}