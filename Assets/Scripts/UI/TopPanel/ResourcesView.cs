using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.TopPanel
{
    public class ResourcesView : View<ResourcesViewModel>
    {
        [SerializeField] private Text _money;
        [SerializeField] private Text _gems;
        [SerializeField] private Button _shopButton;

        private ReactiveEvent _shopEvent = new();
        public IReactiveSubscription ShopEvent => _shopEvent;

        protected override void BindInner(ResourcesViewModel vm)
        {
            _vm.Money.Bind(UpdateMoney).AddTo(this);
            _vm.Gems.Bind(UpdateGems).AddTo(this);
            
            _shopButton.Subscribe(_shopEvent.Trigger).AddTo(this);
        }

        private void UpdateMoney(double value)
        {
            _money.text = LargeNumberFormatter.FormatNumber(value);
        }
        
        private void UpdateGems(int value)
        {
            _gems.text = LargeNumberFormatter.FormatNumber(value);
        }
    }

    public class ResourcesViewModel : ViewModel
    {
        private readonly ReactiveProperty<double> _money = new();
        public IReactiveProperty<double> Money => _money;
        
        private readonly ReactiveProperty<int> _gems = new();
        public IReactiveProperty<int> Gems => _gems;

        public void SetMoney(double value)
        {
            _money.Set(value);
        }
        
        public void SetGems(int value)
        {
            _gems.Set(value);
        }
    }
}