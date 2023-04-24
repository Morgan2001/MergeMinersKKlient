using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class OfflineIncomePopup : Popup<OfflineIncomePopupViewModel>
    {
        [SerializeField] private Text _incomeText;
        [SerializeField] private Text _multipliedIncomeText;
        [SerializeField] private Text _buttonText;
        [SerializeField] private Button _multiplyButton;
        [SerializeField] private Button _skipButton;

        private ReactiveEvent<bool> _clickEvent = new();
        public IReactiveSubscription<bool> ClickEvent => _clickEvent;
        
        protected override void BindInner(OfflineIncomePopupViewModel vm)
        {
            _incomeText.text = LargeNumberFormatter.FormatNumber(_vm.Income);
            _multipliedIncomeText.text = LargeNumberFormatter.FormatNumber(_vm.MultipliedIncome);
            _buttonText.text = "Забрать " + LargeNumberFormatter.FormatNumber(_vm.Income);
            
            _multiplyButton.Subscribe(OnMultiplyClick).AddTo(this);
            _skipButton.Subscribe(OnSkipClick).AddTo(this);
        }

        private void OnMultiplyClick()
        {
            _clickEvent.Trigger(true);
            Hide();
        }
        
        private void OnSkipClick()
        {
            _clickEvent.Trigger(false);
            Hide();
        }
    }

    public class OfflineIncomePopupViewModel : ViewModel
    {
        public double Income { get; }
        public double MultipliedIncome { get; }

        public OfflineIncomePopupViewModel(double income, double multipliedIncome)
        {
            Income = income;
            MultipliedIncome = multipliedIncome;
        }
    }
}