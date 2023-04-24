using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.UpgradesScreen
{
    public class UpgradeView : View<UpgradeViewModel>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _description;
        [SerializeField] private Text _cost;
        [SerializeField] private Button _buyButton;

        private ReactiveEvent<string> _buyEvent = new();
        public IReactiveSubscription<string> BuyEvent => _buyEvent;

        protected override void BindInner(UpgradeViewModel vm)
        {
            _icon.sprite = _vm.Icon;
            _description.text = _vm.Description;
            _cost.text = _vm.Price.ToString();
            
            _buyButton.Subscribe(OnBuyClick).AddTo(this);
        }

        private void OnBuyClick()
        {
            _buyEvent.Trigger(_vm.Id);
        }
    }

    public class UpgradeViewModel : ViewModel
    {
        public string Id { get; }
        public Sprite Icon { get; }
        public string Description { get; }
        public int Price { get; }

        public UpgradeViewModel(string id, Sprite icon, string description, int price)
        {
            Id = id;
            Icon = icon;
            Description = description;
            Price = price;
        }
    }
}