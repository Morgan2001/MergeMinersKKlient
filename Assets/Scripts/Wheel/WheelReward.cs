using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelReward", menuName = "ScriptableObjects/WheelReward", order = 1)]
public class WheelReward : ScriptableObject
{
    [Header("Data")]
    public Boosts BoostType;
    public float Duration;
    public float Power;

    [Header("UI")]
    public Sprite Sprite;
    public string Description;
}
