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

        public static async void AnimateMerge(Func<Transform> getTarget, Action onRotate)
        {
            var rotation = new Vector3(0, 0, 50);
            await getTarget().DORotate(rotation, 0.1f).SetEase(Ease.Linear).Play().ToUniTask();
            onRotate?.Invoke();
            getTarget().eulerAngles = rotation;
            await getTarget().DORotate(Vector3.zero, 0.1f).SetEase(Ease.Linear).Play().ToUniTask();
        }

        public static async void AnimateMaxLevelIncreased(MinerAnimationSetup minerLeft, MinerAnimationSetup minerRight, RectTransform from, RectTransform to, Action onComplete)
        {
            var left = LaunchMiner(minerLeft, from, to);
            var right = LaunchMiner(minerRight, from, to);
            await UniTask.WhenAll(left, right);
            
            onComplete?.Invoke();
        }
        
        private static async UniTask LaunchMiner(MinerAnimationSetup minerAnimationSetup, RectTransform from, RectTransform to)
        {
            minerAnimationSetup.gameObject.SetActive(true);
            
            var anchorTween1 = minerAnimationSetup.transform.DOMove(minerAnimationSetup.Anchor.position, 0.2f).From(from.position).Play().ToUniTask();
            var rotateTween1 = minerAnimationSetup.transform.DORotate(new Vector3(0, 0, minerAnimationSetup.RotationStart), 0.1f).Play().ToUniTask();
            await UniTask.WhenAll(anchorTween1, rotateTween1);
            
            var anchorTween2 = minerAnimationSetup.transform.DOMove(to.position, 0.5f).SetEase(Ease.InCirc).Play().ToUniTask();
            var rotateTween2 = minerAnimationSetup.transform.DORotate(new Vector3(0, 0, minerAnimationSetup.RotationEnd), 0.6f).SetEase(Ease.InSine).Play().ToUniTask();
            await UniTask.WhenAll(anchorTween2, rotateTween2);
            
            minerAnimationSetup.gameObject.SetActive(false);
        }
    }
}