using GameCore.Preloader;
using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class BalancePopup : Popup<BalancePopupViewModel>
    {
        [SerializeField] private Text _balance;
        [SerializeField] private Button _navigateButton;
        
        protected override void BindInner(BalancePopupViewModel vm)
        {
            _balance.text = _vm.Balance.ToString();
            
            _navigateButton.Subscribe(OnNavigate).AddTo(this);
        }

        private void OnNavigate()
        {
            var url = RestClient.BaseUrl + "balance/loginFromGame?token=" + _vm.Token;
            Application.OpenURL(url);
            Hide();
        }
    }

    public class BalancePopupViewModel : ViewModel
    {
        public int Balance { get; }
        public string Token { get; }

        public BalancePopupViewModel(int balance, string token)
        {
            Balance = balance;
            Token = token;
        }
    }
}