using GameCore.Connectors;
using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class EmailPopup : Popup<EmailPopupViewModel>
    {
        [SerializeField] private GameObject _registrationState;
        [SerializeField] private GameObject _registrationStateStep1;
        [SerializeField] private GameObject _registrationStateStep2;
        [SerializeField] private GameObject _loginState;

        [SerializeField] private InputField _registrationEmailInput;
        [SerializeField] private InputField _registrationEmailConfirmInput;
        [SerializeField] private InputField _registrationReferralInput;
        [SerializeField] private Button _registrationNextButton;
        
        [SerializeField] private InputField _registrationPasswordInput;
        [SerializeField] private InputField _registrationPasswordConfirmInput;
        [SerializeField] private Button _registrationDoneButton;
        
        [SerializeField] private Button _registrationLoginButton;
        
        [SerializeField] private InputField _loginEmailInput;
        [SerializeField] private InputField _loginPasswordInput;
        [SerializeField] private Button _loginForgetButton;
        [SerializeField] private Button _loginLoginButton;
        [SerializeField] private Button _loginRegisterButton;
        
        [SerializeField] private Button _closeButton;

        private ReactiveEvent<RegistrationData> _registrationEvent = new();
        public IReactiveSubscription<RegistrationData> RegistrationEvent => _registrationEvent;
        
        private ReactiveEvent<ForgetData> _forgetEvent = new();
        public IReactiveSubscription<ForgetData> ForgetEvent => _forgetEvent;
        
        private ReactiveEvent<LoginData> _loginEvent = new();
        public IReactiveSubscription<LoginData> LoginEvent => _loginEvent;

        protected override void BindInner(EmailPopupViewModel vm)
        {
            _vm.Registered.Bind(UpdateRegistered).AddTo(this);
            _vm.Step.Bind(UpdateStep).AddTo(this);
            
            _registrationNextButton.Subscribe(OnNextClick).AddTo(this);
            _registrationDoneButton.Subscribe(OnDoneClick).AddTo(this);
            _registrationLoginButton.Subscribe(OnAlreadyRegisteredClick).AddTo(this);
            
            _loginForgetButton.Subscribe(OnForgetClick).AddTo(this);
            _loginLoginButton.Subscribe(OnLoginClick).AddTo(this);
            _loginRegisterButton.Subscribe(OnNotRegisteredClick).AddTo(this);
            
            _closeButton.Subscribe(Hide).AddTo(this);
        }

        private void UpdateRegistered(bool value)
        {
            _registrationState.SetActive(!value);
            _loginState.SetActive(value);
        }
        
        private void UpdateStep(RegistrationStep value)
        {
            _registrationStateStep1.SetActive(value == RegistrationStep.Email);
            _registrationStateStep2.SetActive(value == RegistrationStep.Password);
        }

        private void OnNextClick()
        {
            _vm.SetStep(RegistrationStep.Password);
        }
        
        private void OnDoneClick()
        {
            _registrationEvent.Trigger(new RegistrationData(
                _registrationEmailInput.text, 
                _registrationEmailConfirmInput.text,
                _registrationPasswordInput.text,
                _registrationPasswordConfirmInput.text,
                _registrationReferralInput.text
            ));
        }
        
        private void OnForgetClick()
        {
            _forgetEvent.Trigger(new ForgetData(
                _loginEmailInput.text 
            ));
        }
        
        private void OnLoginClick()
        {
            _loginEvent.Trigger(new LoginData(
                _loginEmailInput.text, 
                _loginPasswordInput.text
            ));
        }
        
        private void OnAlreadyRegisteredClick()
        {
            _vm.SetRegistered(true);
        }
        
        private void OnNotRegisteredClick()
        {
            _vm.SetRegistered(false);
        }
    }

    public enum RegistrationStep
    {
        Email,
        Password
    }

    public class EmailPopupViewModel : ViewModel
    {
        private ReactiveProperty<bool> _registered = new();
        public IReactiveProperty<bool> Registered => _registered;
        
        private ReactiveProperty<RegistrationStep> _step = new();
        public IReactiveProperty<RegistrationStep> Step => _step;

        public void SetRegistered(bool value)
        {
            _registered.Set(value);
        }

        public void SetStep(RegistrationStep value)
        {
            _step.Set(value);
        }
    }
}