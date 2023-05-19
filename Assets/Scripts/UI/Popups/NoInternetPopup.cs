using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class NoInternetPopup : Popup<NoInternetPopupViewModel>
    {
        [SerializeField] private Button _button;

        protected override void BindInner(NoInternetPopupViewModel vm)
        {
            _button.Subscribe(OnClick).AddTo(this);
        }

        private void OnClick()
        {
            _vm.ReconnectAction.Invoke();
            Hide();
        }
    }

    public class NoInternetPopupViewModel : ViewModel
    {
        public Action ReconnectAction { get; }

        public NoInternetPopupViewModel(Action reconnectAction)
        {
            ReconnectAction = reconnectAction;
        }
    }
}