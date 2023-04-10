using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.Utils
{
    public static class AnimationHelper
    {
        public static async void AnimateNewBox(RectTransform target, Vector3 from, Vector3 to, Transform container, Action onLand, Action onComplete)
        {
            var parent = target.transform.parent;
            
            target.transform.SetParent(container);
            target.transform.position = from;

            await target.DOScale(new Vector3(1.4f, 1f), 0.1f).Play().ToUniTask();
            await target.DOScale(new Vector3(1f, 1.3f), 0.1f).Play().ToUniTask();

            await target.DOJump(to, 200, 1, 0.2f).SetEase(Ease.Linear).Play().ToUniTask();
            
            onLand?.Invoke();
            
            await target.DOScale(new Vector3(1.4f, 1f), 0.1f).Play().ToUniTask();
            await target.DOScale(new Vector3(1f, 1f), 0.1f).Play().ToUniTask();
            
            target.transform.SetParent(parent);
            
            onComplete?.Invoke();
        }
    }
}