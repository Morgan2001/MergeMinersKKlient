using MergeMiner.Core.State.Config;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyingBonusData", menuName = "ScriptableObjects/FlyingBonusData", order = 1)]
public class FlyingBonusData : ScriptableObject
{
    public BonusType Type;
    public Sprite Sprite;
    public int NumOfBouncesToDestroy;
    public float TimeToFLyBetweenSides;
}
