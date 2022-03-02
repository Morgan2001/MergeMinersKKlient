using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyingBonusData", menuName = "ScriptableObjects/FlyingBonusData", order = 1)]
public class FlyingBonusData : ScriptableObject
{
    public FlyingBonuses Type;
    public Sprite Sprite;
    public int NumOfBouncesToDestroy;
    public float TimeToFLyBetweenSides;
}
