using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.Popups
{
    public class GiftPopup : Popup<GiftPopupViewModel>
    {
        [SerializeField] private Text _gems;
        [SerializeField] private Button _button;

        private readonly ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;

        protected override void BindInner(GiftPopupViewModel vm)
        {
            _gems.text = _vm.Gems.ToString();
            _button.Subscribe(OnClick).AddTo(this);
        }

        private void OnClick()
        {
            _clickEvent.Trigger();

            Hide();
        }
    }

    public class GiftPopupViewModel : ViewModel
    {
        public int Gems { get; }

        public GiftPopupViewModel(int gems)
        {
            Gems = gems;
        }
    }
}