using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WithMergeAnimation : MonoBehaviour
{
    public Image ImageToAnimate;
    public void StartAnimation(Action callback)
    {
        GetComponent<MiningDevice>().IsClickable = false;
        ImageToAnimate.transform.DORotate(new Vector3(0, 0, 50), 0.1f).SetEase(Ease.Linear).OnComplete(() => 
        {
            GetComponent<MiningDevice>().IsClickable = true;
            callback?.Invoke();
        });
    }
}
