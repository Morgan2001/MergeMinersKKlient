using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMiningDeviceAnimation : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject LeftPoint;
    public GameObject RightPoint;
    public GameObject EndPoint;

    public GameObject LeftDevice;
    public GameObject RightDevice;

    private Vector2 StartPointPos;
    private Vector2 LeftPointPos;
    private Vector2 RightPointPos;
    private Vector2 EndPointPos;

    private RelativePositionsCalculator relPosCalc;
    private GameRules gameRules;

    public void Construct(RelativePositionsCalculator relPosCalc, GameRules gameRules)
    {
        this.relPosCalc = relPosCalc;
        this.gameRules = gameRules;
    }

    public void StartAnimation(MiningDeviceData newDeviceData, Action callback)
    {
        CalcPositions();
        SetSprites(newDeviceData);
        LaunchLeftDevice();
        LaunchRightDevice(callback.Invoke);
    }

    private void SetSprites(MiningDeviceData newDeviceData)
    {
        LeftDevice.GetComponent<Image>().sprite = gameRules.MiningDevices[newDeviceData.Level - 1].Sprite;
        RightDevice.GetComponent<Image>().sprite = gameRules.MiningDevices[newDeviceData.Level - 1].Sprite;
    }

    private Sequence LaunchLeftDevice()
    {
        var leftDeviceSeq = DOTween.Sequence();

        LeftDevice.SetActive(true);

        leftDeviceSeq.Append(LeftDevice.GetComponent<RectTransform>().DOAnchorPos(LeftPointPos, 0.2f).From(StartPointPos));
        leftDeviceSeq.Join(LeftDevice.transform.DORotate(new Vector3(0, 0, 45), 0.1f));
        leftDeviceSeq.Append(LeftDevice.GetComponent<RectTransform>().DOAnchorPos(EndPointPos, 0.5f).SetEase(Ease.InCirc));
        leftDeviceSeq.Join(LeftDevice.transform.DORotate(new Vector3(0, 0, -90), 0.6f).SetEase(Ease.InSine));

        leftDeviceSeq.OnComplete(() => HideLeftDevice());
        return leftDeviceSeq;
    }

    private void HideLeftDevice()
    {
        LeftDevice.SetActive(false);
    }

    private Sequence LaunchRightDevice(Action callback)
    {
        var rightDeviceSeq = DOTween.Sequence();

        RightDevice.SetActive(true);

        rightDeviceSeq.Append(RightDevice.GetComponent<RectTransform>().DOAnchorPos(RightPointPos, 0.2f).From(StartPointPos));
        rightDeviceSeq.Join(RightDevice.transform.DORotate(new Vector3(0, 0, -45), 0.1f));
        rightDeviceSeq.Append(RightDevice.GetComponent<RectTransform>().DOAnchorPos(EndPointPos, 0.5f).SetEase(Ease.InCirc));
        rightDeviceSeq.Join(RightDevice.transform.DORotate(new Vector3(0, 0, 90), 0.6f).SetEase(Ease.InSine));

        rightDeviceSeq.OnComplete(() => 
        {
            HideRightDevice();
            callback.Invoke();
        });
        return rightDeviceSeq;
    }

    private void HideRightDevice()
    {
        RightDevice.SetActive(false);
    }

    private void CalcPositions()
    {
        StartPointPos = relPosCalc.CalcRelativePosition(StartPoint.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>());
        LeftPointPos = relPosCalc.CalcRelativePosition(LeftPoint.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>());
        RightPointPos = relPosCalc.CalcRelativePosition(RightPoint.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>());
        EndPointPos = relPosCalc.CalcRelativePosition(EndPoint.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>());
    }
}
