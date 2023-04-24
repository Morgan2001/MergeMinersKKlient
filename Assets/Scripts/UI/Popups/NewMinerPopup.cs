using DG.Tweening;
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

            _minerLeft.Icon.sprite = _vm.PreviousIcon;
            _minerRight.Icon.sprite = _vm.PreviousIcon;
            AnimationHelper.AnimateMaxLevelIncreased(_minerLeft, _minerRight, _from, _to, StartScaleAnimation);
        
            _button.Subscribe(Hide).AddTo(this);
        }

        private void StartScaleAnimation()
        {
            _miner.sprite = _vm.Icon;

            _miner.gameObject.SetActive(true);
            _miner.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutSine).OnComplete(() => 
            { 
                UpdateInfo();
                _button.gameObject.SetActive(true);
            });
        }

        private void UpdateInfo()
        {
            _level.text = _vm.Level.ToString();
            _name.text = _vm.Name;
            _coinsPerSecond.text = LargeNumberFormatter.FormatNumber(_vm.Income) + " per second";
        }
    }

    public class NewMinerPopupViewModel : ViewModel
    {
        public string Name { get; }
        public int Level { get; }
        public double Income { get; }
        public Sprite Icon { get; }
        public Sprite PreviousIcon { get; }

        public NewMinerPopupViewModel(string name, int level, double income, Sprite icon, Sprite previousIcon)
        {
            Name = name;
            Level = level;
            Income = income;
            Icon = icon;
            PreviousIcon = previousIcon;
        }
    }
}