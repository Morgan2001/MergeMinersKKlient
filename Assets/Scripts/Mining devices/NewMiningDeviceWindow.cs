using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewMiningDeviceWindow : MonoBehaviour
{
    public Image MiningDeviceImage;
    public Text MiningDeviceLevel;
    public Text MiningDeviceName;
    public Text CoinsPerSecond;
    public string PerSecondString;
    public Button AcceptButton;

    private MiningDeviceData miningDeviceData;

    public NewMiningDeviceAnimation newMiningDeviceAnimation;

    public void ShowNewDeviceWithAnimation(MiningDeviceData miningDeviceData)
    {
        this.miningDeviceData = miningDeviceData;

        MiningDeviceImage.gameObject.SetActive(false);
        AcceptButton.gameObject.SetActive(false);

        newMiningDeviceAnimation.StartAnimation(miningDeviceData, StartNewDeviceAnimation);
    }

    private void SetDataAboutNewDevice()
    {
        MiningDeviceLevel.text = miningDeviceData.Level.ToString();
        MiningDeviceName.text = miningDeviceData.Name;
        CoinsPerSecond.text = miningDeviceData.CoinsPerSecond + " " + PerSecondString;
    }

    private void StartNewDeviceAnimation()
    {
        MiningDeviceImage.sprite = miningDeviceData.Sprite;

        MiningDeviceImage.gameObject.SetActive(true);
        MiningDeviceImage.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutSine).OnComplete(() => 
        { 
            SetDataAboutNewDevice();
            AcceptButton.gameObject.SetActive(true);
        });
    }
}
