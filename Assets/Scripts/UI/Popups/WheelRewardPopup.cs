using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class WheelRewardPopup : Popup<WheelRewardPopupViewModel>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _description;
        [SerializeField] private Button _button;
        
        protected override void BindInner(WheelRewardPopupViewModel vm)
        {
            _icon.sprite = _vm.Icon;
            _description.text = _vm.Description;
            
            _button.Subscribe(Hide).AddTo(this);
        }
    }

    public class WheelRewardPopupViewModel : ViewModel
    {
        public Sprite Icon { get; }
        public string Description { get; }

        public WheelRewardPopupViewModel(Sprite icon, string description)
        {
            Icon = icon;
            Description = description;
        }
    }
}