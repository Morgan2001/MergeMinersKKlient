using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.BottomPanel
{
    public class BlueBoxView : View<BlueBoxViewModel>
    {
        [SerializeField] private Image _progress;
        [SerializeField] private Button _button;

        protected override void BindInner(BlueBoxViewModel vm)
        {
            _vm.Progress.Bind(UpdateProgress).AddTo(this);
            
            _button.Subscribe(_vm.ButtonClick).AddTo(this);
        }

        private void UpdateProgress(float value)
        {
            _progress.fillAmount = value;
        }
    }

    public class BlueBoxViewModel : ViewModel
    {
        private ReactiveProperty<float> _progress = new();
        public IReactiveProperty<float> Progress => _progress;

        private ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;

        public void ButtonClick()
        {
            _clickEvent.Trigger();
        }

        public void SetProgress(float value)
        {
            _progress.Set(value);
        }
    }
}