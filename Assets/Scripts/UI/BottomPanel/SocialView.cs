using Utils;
using Utils.MVVM;

namespace UI.BottomPanel
{
    public class SocialView : View<SocialViewModel>
    {
        protected override void BindInner(SocialViewModel vm)
        {
            _vm.IsShown.Bind(OnShownUpdate).AddTo(this);
        }

        private void OnShownUpdate(bool value)
        {
            gameObject.SetActive(value);
        }
    }

    public class SocialViewModel : ViewModel
    {
        private readonly ReactiveProperty<bool> _isShown = new();
        public IReactiveProperty<bool> IsShown => _isShown;

        public void SetShown(bool value)
        {
            _isShown.Set(value);
        }
    }
}