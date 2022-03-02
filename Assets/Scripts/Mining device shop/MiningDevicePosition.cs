using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningDevicePosition : MonoBehaviour
{
    public Text Price;
    public Text DeviceLevel;
    public Image DeviceImage;

    public void SetOffer(double price, MiningDeviceData miningDeviceData)
    {
        if (Price != null)
        {
            Price.text = LargeNumberFormatter.FormatNumber(price) + " ₿";
        }
        DeviceLevel.text = miningDeviceData.Level.ToString();
        DeviceImage.sprite = miningDeviceData.Sprite;
    }
}
