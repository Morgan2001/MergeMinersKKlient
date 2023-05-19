using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.TopPanel
{
    public class FilledButtonView : View<FilledButtonViewModel>
    {
        [SerializeField] private Image _highlight;
        [SerializeField] private Image _progress;
        [SerializeField] private Button _button;
        
        protected override void BindInner(FilledButtonViewModel vm)
        {
            _vm.Progress.Bind(OnProgressUpdate).AddTo(this);
            
            _button.Subscribe(_vm.ButtonClick).AddTo(this);
        }

        private void OnProgressUpdate(float value)
        {
            _highlight.enabled = value >= 1f;
            _progress.fillAmount = value;
        }
    }

    public class FilledButtonViewModel : ViewModel
    {
        private readonly ReactiveProperty<float> _progress = new();
        public IReactiveProperty<float> Progress => _progress;

        private readonly ReactiveEvent _buttonClickEvent = new();
        public IReactiveSubscription ButtonClickEvent => _buttonClickEvent;

        public void ButtonClick()
        {
            _buttonClickEvent.Trigger();
        }

        public void SetProgress(float value)
        {
            _progress.Set(value);
        }
    }
}