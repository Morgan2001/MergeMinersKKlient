using DG.Tweening;
using System;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    
    private RelativePositionsCalculator _relativePositionsCalculator;

    public void Construct(RelativePositionsCalculator relativePositionsCalculator)
    {
        _relativePositionsCalculator = relativePositionsCalculator;
    }

    public void Jump(RectTransform whatWillJump, RectTransform to, float jumpPower, float duration, Action action, RectTransform from = null)
    {
        whatWillJump.GetComponent<MiningDevice>().IsClickable = false;

        whatWillJump.SetParent(transform);

        var posFromJump = _relativePositionsCalculator.CalcRelativePosition(from == null ? whatWillJump : from, _rectTransform);
        var posToJump = _relativePositionsCalculator.CalcRelativePosition(to, _rectTransform);

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
