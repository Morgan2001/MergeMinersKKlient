using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.Popups
{
    public class RouletteWinPopup : Popup<RouletteWinPopupViewModel>
    {
        [SerializeField] private Text _level;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        protected override void BindInner(RouletteWinPopupViewModel vm)
        {
            _level.text = _vm.Level.ToString();
            _icon.sprite = _vm.Icon;
            
            _button.Subscribe(Hide).AddTo(this);
        }
    }

    public class RouletteWinPopupViewModel : ViewModel
    {
        public int Level { get; }
        public Sprite Icon { get; }

        public RouletteWinPopupViewModel(int level, Sprite icon)
        {
            Level = level;
            Icon = icon;
        }
    }
}