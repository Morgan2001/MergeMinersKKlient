using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.ShopScreen
{
    public class ShopPurchaseView : View<ShopPurchaseViewModel>
    {
        [SerializeField] private Text _gems;
        [SerializeField] private Text _price;
        [SerializeField] private Button _button;

        private ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;
        
        protected override void BindInner(ShopPurchaseViewModel vm)
        {
            _gems.text = _vm.Gems.ToString();
            _price.text = "$" + _vm.Price;
            
            _button.Subscribe(_clickEvent.Trigger).AddTo(this);
        }
    }

    public class ShopPurchaseViewModel : ViewModel
    {
        public int Gems { get; }
        public double Price { get; }

        public ShopPurchaseViewModel(int gems, double price)
        {
            Gems = gems;
            Price = price;
        }
    }
}