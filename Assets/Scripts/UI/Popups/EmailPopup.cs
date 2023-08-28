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
        [SerializeField] private GameObject _loggedState;

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
        
        [SerializeField] private Text _email;
        [SerializeField] private Button _logoutButton;

        [SerializeField] private Button _deleteButton;

        [SerializeField] private Button _closeButton;

        private ReactiveEvent<RegistrationData> _registrationEvent = new();
        public IReactiveSubscription<RegistrationData> RegistrationEvent => _registrationEvent;
        
        private ReactiveEvent<ForgetData> _forgetEvent = new();
        public IReactiveSubscription<ForgetData> ForgetEvent => _forgetEvent;
        
        private ReactiveEvent<LoginData> _loginEvent = new();
        public IReactiveSubscription<LoginData> LoginEvent => _loginEvent;
        
        private ReactiveEvent _logoutEvent = new();
        public IReactiveSubscription LogoutEvent => _logoutEvent;

        private ReactiveEvent _deleteEvent = new();
        public IReactiveSubscription DeleteEvent => _deleteEvent;

        protected override void BindInner(EmailPopupViewModel vm)
        {
            _vm.State.Bind(UpdateState).AddTo(this);
            _vm.Step.Bind(UpdateStep).AddTo(this);
            
            _registrationNextButton.Subscribe(OnNextClick).AddTo(this);
            _registrationDoneButton.Subscribe(OnDoneClick).AddTo(this);
            _registrationLoginButton.Subscribe(OnAlreadyRegisteredClick).AddTo(this);
            
            _loginForgetButton.Subscribe(OnForgetClick).AddTo(this);
            _loginLoginButton.Subscribe(OnLoginClick).AddTo(this);
            _loginRegisterButton.Subscribe(OnNotRegisteredClick).AddTo(this);

            _email.text = _vm.Email;
            _logoutButton.Subscribe(OnLogoutClick).AddTo(this);
            _closeButton.Subscribe(Hide).AddTo(this);
            _deleteButton.Subscribe(OnDeleteClick).AddTo(this);
        }

        private void OnDeleteClick()
        {
            _deleteEvent.Trigger();
            _vm.SetState(EmailState.Registration);

        }



        private void UpdateState(EmailState value)
        {
            _registrationState.SetActive(value == EmailState.Registration);
            _loginState.SetActive(value == EmailState.Login);
            _loggedState.SetActive(value == EmailState.Logged);
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
            _vm.SetState(EmailState.Logged);
        }
        
        private void OnLogoutClick()
        {
            _logoutEvent.Trigger();
            _vm.SetState(EmailState.Login);
        }
        
        private void OnAlreadyRegisteredClick()
        {
            _vm.SetState(EmailState.Login);
        }
        
        private void OnNotRegisteredClick()
        {
            _vm.SetState(EmailState.Registration);
        }
    }

    public enum RegistrationStep
    {
        Email,
        Password
    }

    public class EmailPopupViewModel : ViewModel
    {
        public string Email { get; }
        
        private ReactiveProperty<EmailState> _state = new();
        public IReactiveProperty<EmailState> State => _state;
        
        private ReactiveProperty<RegistrationStep> _step = new();
        public IReactiveProperty<RegistrationStep> Step => _step;

        public EmailPopupViewModel(string email)
        {
            Email = email;
            
            SetState(string.IsNullOrEmpty(Email) ? 
                EmailState.Registration : 
                EmailState.Logged);
        }

        public void SetState(EmailState value)
        {
            _state.Set(value);
        }

        public void SetStep(RegistrationStep value)
        {
            _step.Set(value);
        }
    }

    public enum EmailState
    {
        Registration,
        Login,
        Logged
    }
}