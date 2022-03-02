using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    RelativePositionsCalculator relPosCalculator;

    public void Construct(RelativePositionsCalculator relPosCalculator)
    {
        this.relPosCalculator = relPosCalculator;
    }

    public void Jump(Transform whatWillJump, Transform to, float jumpPower, float duration, Action action, Transform from = null)
    {
        whatWillJump.GetComponent<MiningDevice>().IsClickable = false;

        whatWillJump.SetParent(transform);

        var posFromJump = relPosCalculator.CalcRelativePosition(from == null ? whatWillJump.gameObject : from.gameObject, gameObject);
        var posToJump = relPosCalculator.CalcRelativePosition(to.gameObject, gameObject);



        whatWillJump.GetComponent<RectTransform>().anchoredPosition = posFromJump;

        whatWillJump.GetComponent<RectTransform>().DOJumpAnchorPos(posToJump, jumpPower, 1, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                whatWillJump.GetComponent<MiningDevice>().IsClickable = true;
                action.Invoke();
            }).SetLink(whatWillJump.gameObject);
    }
}
