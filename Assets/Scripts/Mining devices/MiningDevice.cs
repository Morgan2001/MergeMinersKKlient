using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiningDevice : MonoBehaviour
{
    public GameObject Label;
    public Text LevelText;
    public Image Image;

    public bool IsClickable { get; set; } = true;

    private MiningDeviceData data;
    
    public MiningDeviceData Data
    {
        get => data;
        private set
        {
            data = value;
            SetCommonSprite();
            Label.SetActive(true);
            LevelText.text = data.Level.ToString();
        }
    }

    public void Construct(MiningDeviceData data)
    {
        Data = data;
    }
    public void UpdateMiningDevice(MiningDeviceData newData)
    {
        Data = newData;
    }
    public void SetOutlinedSprite()
    {
        Image.sprite = data.SpriteWithOutline;
    }
    public void SetCommonSprite()
    {
        Image.sprite = data.Sprite;
    }
}
