using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelRewardPanel : MonoBehaviour
{
    public Image Image;
    public Text Description;

    public void SetData(Sprite sprite, string description)
    {
        Image.sprite = sprite;
        Description.text = description;
    }
}
