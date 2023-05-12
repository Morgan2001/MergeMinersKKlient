using System;
using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class AlertPopup : Popup<AlertPopupViewModel>
    {
        [SerializeField] private Text _text;
        [SerializeField] private Text _buttonLabel;
        [SerializeField] private Button _button;
        
        protected override void BindInner(AlertPopupViewModel vm)
        {
            _text.text = _vm.Text;
            _buttonLabel.text = _vm.ButtonLabel;

            _button.Subscribe(_vm.ButtonAction ?? Hide).AddTo(this);
        }
    }

    public class AlertPopupViewModel : ViewModel
    {
        public string Text { get; }
        public string ButtonLabel { get; }
        public Action ButtonAction { get; }

        public AlertPopupViewModel(string text, string buttonLabel, Action buttonAction)
        {
            Text = text;
            ButtonLabel = buttonLabel;
            ButtonAction = buttonAction;
        }
    }
}