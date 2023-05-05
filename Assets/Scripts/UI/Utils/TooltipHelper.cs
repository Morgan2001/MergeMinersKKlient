using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utils
{
    public static class TooltipHelper
    {
        private static float ScaleHover = 1.25f;
        private static float PunchPower = 0.25f;
        
        public static async void AnimateTap(Image tooltip, Vector3 target, int count, Action callback, CancellationToken token)
        {
            tooltip.color = Color.clear;
            tooltip.transform.position = target;
            tooltip.transform.localScale = ScaleHover * Vector3.one;

            try
            {
                await tooltip.DOColor(Color.white, 0.3f).Play().ToUniTask(cancellationToken: token);

                for (int i = 0; i < count; i++)
                {
                    if (i > 0) await UniTask.Delay(100, cancellationToken: token);
                    await tooltip.transform.DOPunchScale(-PunchPower * Vector3.one, 0.3f, 3, 0.25f).Play().ToUniTask(cancellationToken: token);
                }

                await tooltip.DOColor(Color.clear, 0.3f).Play().ToUniTask(cancellationToken: token);
                
                if (!token.IsCancellationRequested) callback?.Invoke();
            }
            catch (OperationCanceledException)
            {
                tooltip.color = Color.clear;
            }
        }
        
        public static async void AnimateMove(Image tooltip, Vector3 from, Vector3 to, Action callback, CancellationToken token)
        {
            tooltip.color = Color.clear;
            tooltip.transform.position = from;
            tooltip.transform.localScale = ScaleHover * Vector3.one;

            try
            {
                await tooltip.DOColor(Color.white, 0.3f).Play().ToUniTask(cancellationToken: token);

                await tooltip.transform.DOScale(Vector3.one, 0.5f).Play().ToUniTask(cancellationToken: token);
                await tooltip.transform.DOMove(to, 0.5f).Play().ToUniTask(cancellationToken: token);
                await tooltip.transform.DOScale(ScaleHover * Vector3.one, 0.5f).Play()
                    .ToUniTask(cancellationToken: token);

                await tooltip.DOColor(Color.clear, 0.3f).Play().ToUniTask(cancellationToken: token);
                
                if (!token.IsCancellationRequested) callback?.Invoke();
            }
            catch (OperationCanceledException)
            {
                tooltip.color = Color.clear;
            }
        }
    }
}