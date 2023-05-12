using DG.Tweening;
using I2.Loc;
using UI.Utils;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class NewMinerPopup : Popup<NewMinerPopupViewModel>
    {
        [SerializeField] private Image _miner;
        [SerializeField] private Text _level;
        [SerializeField] private Text _name;
        [SerializeField] private Text _coinsPerSecond;
        [SerializeField] private Button _button;
    
        [SerializeField] private MinerAnimationSetup _minerLeft;
        [SerializeField] private MinerAnimationSetup _minerRight;
        [SerializeField] private RectTransform _from;
        [SerializeField] private RectTransform _to;
    
        protected override void BindInner(NewMinerPopupViewModel vm)
        {
            _miner.gameObject.SetActive(false);
            _button.gameObject.SetActive(false);
            
            UpdateInfo(_vm.PreviousMiner);

            _minerLeft.Icon.sprite = _vm.PreviousMiner.Item4;
            _minerRight.Icon.sprite = _vm.PreviousMiner.Item4;
            AnimationHelper.AnimateMaxLevelIncreased(_minerLeft, _minerRight, _from, _to, StartScaleAnimation);
        
            _button.Subscribe(Hide).AddTo(this);
        }

        private void StartScaleAnimation()
        {
            _miner.sprite = _vm.NewMiner.Item4;

            _miner.gameObject.SetActive(true);
            _miner.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutSine).OnComplete(() => 
            { 
                UpdateInfo(_vm.NewMiner);
                _button.gameObject.SetActive(true);
            });
        }

        private void UpdateInfo((string, int, double, Sprite) miner)
        {
            _level.text = miner.Item2.ToString();
            _name.text = LocalizationManager.GetTranslation(miner.Item1);

            var text = LocalizationManager.GetTranslation("text-earn-per-second");
            _coinsPerSecond.text = string.Format(text, LargeNumberFormatter.FormatNumber(miner.Item3));
        }
    }

    public class NewMinerPopupViewModel : ViewModel
    {
        public (string, int, double, Sprite) PreviousMiner { get; }
        public (string, int, double, Sprite) NewMiner { get; }

        public NewMinerPopupViewModel((string, int, double, Sprite) previousMiner, (string, int, double, Sprite) newMiner)
        {
            PreviousMiner = previousMiner;
            NewMiner = newMiner;
        }
    }
}