using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeAnimation : MonoBehaviour
{
    public Image ImageToShake;
    private Tween tween;

    public bool IsOn { get; private set; } = false;

    public void StartShaking()
    {
        if (!IsOn)
        {
            tween = ImageToShake.transform.DOShakePosition(0.5f, new Vector3(20, 10, 0), 10, 90, false).SetEase(Ease.Linear).SetLoops(int.MaxValue).SetLink(gameObject);
            IsOn = true;
        }
    }
    public void StopShaking()
    {
        IsOn = false;
        tween.Kill();
    }
}
