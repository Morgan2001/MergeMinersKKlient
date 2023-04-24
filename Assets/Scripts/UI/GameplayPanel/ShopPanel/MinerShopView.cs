using GameCore.Connectors;
using MergeMiner.Core.State.Data;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.GameplayPanel.ShopPanel
{
    public class MinerShopView : View<MinerShopViewModel>
    {
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;
        
        [SerializeField] private Sprite _backgroundMoneyNormal;
        [SerializeField] private Sprite _backgroundMoneyPressed;
        [SerializeField] private Sprite _backgroundAdsNormal;
        [SerializeField] private Sprite _backgroundAdsPressed;
        [SerializeField] private Sprite _backgroundGemsNormal;
        [SerializeField] private Sprite _backgroundGemsPressed;
        
        [SerializeField] private GameObject _money;
        [SerializeField] private GameObject _ads;
        [SerializeField] private GameObject _gem;
        
        [SerializeField] private Image _icon;
        [SerializeField] private Text _level;
        [SerializeField] private Text _price;
        
        protected override void BindInner(MinerShopViewModel vm)
        {
            _level.text = _vm.Level.ToString();
            _icon.sprite = _vm.Icon;

            _vm.Currency.Bind(UpdateCurrency).AddTo(this);
            _vm.Price.Bind(UpdatePrice).AddTo(this);
            _button.Subscribe(_vm.ButtonClick).AddTo(this);
        }

        private void UpdateCurrency(Currency value)
        {
            switch (value)
            {
                case Currency.Money:
                {
                    _background.sprite = _backgroundMoneyNormal;
                    _button.spriteState = new SpriteState { pressedSprite = _backgroundMoneyPressed };
                    break;
                }
                case Currency.Ads:
                {
                    _background.sprite = _backgroundAdsNormal;
                    _button.spriteState = new SpriteState { pressedSprite = _backgroundAdsPressed };
                    break;
                }
                case Currency.Gems:
                {
                    _background.sprite = _backgroundGemsNormal;
                    _button.spriteState = new SpriteState { pressedSprite = _backgroundGemsPressed };
                    break;
                }
            }
            
            _money.SetActive(value != Currency.Ads);
            _ads.SetActive(value == Currency.Ads);
            _gem.SetActive(value == Currency.Gems);
        }

        private void UpdatePrice(double value)
        {
            switch (_vm.Currency.Value)
            {
                case Currency.Money:
                case Currency.Gems:
                {
                    _price.text = LargeNumberFormatter.FormatNumber(value);
                    break;
                }
                default:
                {
                    _price.text = "";
                    break;
                }
            }
        }
    }

    public class MinerShopViewModel : ViewModel
    {
        public string Id { get; }
        
        public int Level { get; }
        public Sprite Icon { get; }
        
        private readonly ReactiveProperty<double> _price = new();
        public IReactiveProperty<double> Price => _price;
        
        private readonly ReactiveProperty<Currency> _currency = new();
        public IReactiveProperty<Currency> Currency => _currency;

        private readonly ReactiveEvent _clickEvent = new();
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

        public void SetPrice(Currency currency, double value)
        {
            _currency.Set(currency);
            _price.Set(value);
        }
    }
}