using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.GameplayPanel.ShopPanel
{
    public class MinerShopView : View<MinerShopViewModel>
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private Text _level;
        [SerializeField] private Text _price;
        
        protected override void BindInner(MinerShopViewModel vm)
        {
            _level.text = _vm.Level.ToString();
            _icon.sprite = _vm.Icon;

            _vm.Price.Bind(UpdatePrice).AddTo(this);
            _button.Subscribe(_vm.ButtonClick).AddTo(this);
        }

        private void UpdatePrice(double value)
        {
            _price.text = LargeNumberFormatter.FormatNumber(value);
        }
    }

    public class MinerShopViewModel : ViewModel
    {
        public string Id { get; }
        
        public int Level { get; }
        public Sprite Icon { get; }
        
        private ReactiveProperty<double> _price = new();
        public IReactiveProperty<double> Price => _price;

        private ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;

        public MinerShopViewModel(string id, int level, Sprite icon)
        {
            Id = id;
            Level = level;
            Icon = icon;
        }

        public void ButtonClick()
        {
            _clickEvent.Trigger();
        }

        public void SetPrice(double value)
        {
            _price.Set(value);
        }
    }
}