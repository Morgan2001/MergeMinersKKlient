using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetOfMiningDeviceBoxes", menuName = "ScriptableObjects/SetOfMiningDeviceBoxes", order = 2)]
public class SetOfMiningDeviceBoxes : ScriptableObject
{
    public List<MiningDeviceBoxSprite> MiningDeviceBoxSprites;
}