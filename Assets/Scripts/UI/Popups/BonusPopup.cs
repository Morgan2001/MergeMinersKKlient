using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.Popups
{
    public class BonusPopup : Popup<BonusPopupViewModel>
    {
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _closeButton;

        private ReactiveEvent _acceptEvent = new();
        public IReactiveSubscription AcceptEvent => _acceptEvent;
        
        protected override void BindInner(BonusPopupViewModel vm)
        {
            _acceptButton.Subscribe(OnAccept).AddTo(this);
            _closeButton.Subscribe(Hide).AddTo(this);
        }

        private void OnAccept()
        {
            _acceptEvent.Trigger();
            Hide();
        }
    }

    public class BonusPopupViewModel : ViewModel
    {
    }
}