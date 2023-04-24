using UnityEngine;

[CreateAssetMenu(fileName = "WheelReward", menuName = "ScriptableObjects/WheelReward", order = 1)]
public class WheelReward : ScriptableObject
{
    public int Id;

    [Header("UI")]
    public Sprite Sprite;
    public string Description;
}