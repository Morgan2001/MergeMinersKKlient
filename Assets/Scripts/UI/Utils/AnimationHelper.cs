using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Utils
{
    public static class AnimationHelper
    {
        public static void AnimateNewBox(Transform container, RectTransform target, RectTransform miningDevice, RectTransform cell)
        {
            miningDevice.SetParent(target);
            miningDevice.anchoredPosition = Vector2.zero;
            miningDevice.SetParent(container);
            
            var seq = DOTween.Sequence();
            seq.Append(target.DOScale(new Vector3(1.4f, 1f), 0.1f));
            seq.Join(target.DOAnchorPos(target.anchoredPosition - new Vector2(0, 50), 0.2f));
            seq.Append(target.DOScale(new Vector3(1f, 1.3f), 0.1f));

            Jump(container, miningDevice, cell, 200, 0.2f, () =>
            {
            });
        }
        
        private static void Jump(Transform container, RectTransform whatWillJump, RectTransform to, float jumpPower, float duration, Action action, Transform from = null)
        {
            whatWillJump.SetParent(container);

            var fromPosition = from == null ? whatWillJump.position : from.position;
            var toPosition = to.position;

            whatWillJump.anchoredPosition = fromPosition;

            whatWillJump.DOJumpAnchorPos(toPosition, jumpPower, 1, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
                {
                    action.Invoke();
                }).SetLink(whatWillJump.gameObject);
        }
    }
}