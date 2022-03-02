using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhatMergeAnimation : MonoBehaviour
{
    public Image ImageToAnimate;

    public void StartAnimation()
    {
        GetComponent<MiningDevice>().IsClickable = false;

        ImageToAnimate.DOFade(0, 0);
        ImageToAnimate.transform.DORotate(Vector3.zero , 0.1f).SetEase(Ease.Linear).From(new Vector3(0, 0, 50)).SetDelay(0.1f);
        ImageToAnimate.DOFade(1, 0.1f).From(1).SetDelay(0.1f).OnComplete(() => { GetComponent<MiningDevice>().IsClickable = true; } );
    }
}
