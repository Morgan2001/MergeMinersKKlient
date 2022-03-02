using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteWinWindow : MonoBehaviour
{
    public Image image;
    public Text levelText;

    public void SetMiningDeviceImage(MiningDeviceData miningDeviceData)
    {
        image.sprite = miningDeviceData.Sprite;
        levelText.text = miningDeviceData.Level.ToString();
    }
}
