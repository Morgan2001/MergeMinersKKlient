using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.Popups
{
    public class RelocationPopup : Popup<RelocationPopupViewModel>
    {
        [SerializeField] private Image _image;
        
        [SerializeField] private Text _name;
        [SerializeField] private Text _level;

        [SerializeField] private Text _slots;
        [SerializeField] private Text _powered;
        [SerializeField] private Text _maxMinerLevel;

        [SerializeField] private Text _buttonText;
        [SerializeField] private Button _relocateButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Color _activeTextColor;
        [SerializeField] private Color _inactiveTextColor;

        private readonly ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;
        
        protected override void BindInner(RelocationPopupViewModel vm)
        {
            _image.sprite = _vm.Image;
            _name.text = _vm.Name;
            _level.text = _vm.Level.ToString();
            _slots.text = _vm.Slots.ToString();
            _powered.text = _vm.Powered.ToString();
            _maxMinerLevel.text = _vm.MaxMinerLevel.ToString();
            
            if (_vm.CurrentMinerLevel < _vm.MinMinerLevelNeeded)
            {
                SetButtonActive(false);
                _buttonText.text = $"Assemble level {_vm.MinMinerLevelNeeded} miner first";
            }
            else if (_vm.CurrentMoney < _vm.RelocateCost)
            {
                SetButtonActive(false);
                _buttonText.text = "Relocate\n" + $"{LargeNumberFormatter.FormatNumber(_vm.RelocateCost)} coins";
            }
            else
            {
                SetButtonActive(true);
                _buttonText.text = "Relocate\n" + $"{LargeNumberFormatter.FormatNumber(_vm.RelocateCost)} coins";
            }

            _relocateButton.Subscribe(OnClick).AddTo(this);
            _closeButton.Subscribe(Hide).AddTo(this);
        }

        private void OnClick()
        {
            _clickEvent.Trigger();
            
            Hide();
        }

        private void SetButtonActive(bool value)
        {
            _relocateButton.interactable = value;
            _buttonText.color = value ? _activeTextColor : _inactiveTextColor;
        }
    }

    public class RelocationPopupViewModel : ViewModel
    {
        public Sprite Image { get; }
        public string Name { get; }
        public int Level { get; }
        public int Slots { get; }
        public int Powered { get; }
        public int MaxMinerLevel { get; }
        public int MinMinerLevelNeeded { get; }
        public int CurrentMinerLevel { get; }
        public double CurrentMoney { get; }
        public double RelocateCost { get; }

        public RelocationPopupViewModel(Sprite image, string name, int level, int slots, int powered, int maxMinerLevel, int minMinerLevelNeeded, int currentMinerLevel, double currentMoney, double relocateCost)
        {
            Image = image;
            Name = name;
            Level = level;
            Slots = slots;
            Powered = powered;
            MaxMinerLevel = maxMinerLevel;
            MinMinerLevelNeeded = minMinerLevelNeeded;
            CurrentMinerLevel = currentMinerLevel;
            CurrentMoney = currentMoney;
            RelocateCost = relocateCost;
        }
    }
}