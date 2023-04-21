using Cysharp.Threading.Tasks;
using DG.Tweening;
using MergeMiner.Core.State.Data;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.Popups
{
    public class WheelPopup : Popup<WheelPopupViewModel>
    {
        [SerializeField] private int _rewards = 6;
        [SerializeField] private int _turns = 5;
        
        [SerializeField] private Button _spinForAdsButton;
        [SerializeField] private Button _spinForGemsButton;
        [SerializeField] private Button _closeButton;
        
        [SerializeField] private GameObject _buttonsPanel;
        [SerializeField] private Transform _wheel;

        private ReactiveEvent<Currency> _spinEvent = new();
        public IReactiveSubscription<Currency> SpinEvent => _spinEvent;
        
        private ReactiveEvent<int> _endSpinEvent = new();
        public IReactiveSubscription<int> EndSpinEvent => _endSpinEvent;

        protected override void BindInner(WheelPopupViewModel vm)
        {
            _vm.SpinEvent.Subscribe(OnSpin).AddTo(this);
            
            _spinForAdsButton.Subscribe(SpinForAds).AddTo(this);
            _spinForGemsButton.Subscribe(SpinForGems).AddTo(this);
            _closeButton.Subscribe(Hide).AddTo(this);
        }

        private async void OnSpin(int reward)
        {
            _buttonsPanel.SetActive(false);
            
            var reversedNum = _rewards - reward;
            var rewardDegrees = 360 * reversedNum / _rewards;

            await _wheel.DORotate(new Vector3(0, 0, -(360 * _turns + rewardDegrees)), 5, RotateMode.FastBeyond360).SetEase(Ease.InOutCubic).Play().ToUniTask();
            await UniTask.Delay(1000);

            _buttonsPanel.SetActive(true);
            _endSpinEvent.Trigger(reward);
        }

        private void SpinForAds()
        {
            _spinEvent.Trigger(Currency.Ads);
        }

        private void SpinForGems()
        {
            _spinEvent.Trigger(Currency.Gems);
        }
    }

    public class WheelPopupViewModel : ViewModel
    {
        private ReactiveEvent<int> _spinEvent = new();
        public IReactiveSubscription<int> SpinEvent => _spinEvent;

        public void Spin(int id)
        {
            _spinEvent.Trigger(id);
        }
    }
}