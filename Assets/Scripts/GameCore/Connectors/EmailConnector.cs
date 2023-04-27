using GameCore.Preloader;
using UnityEngine;

namespace GameCore.Connectors
{
    public class EmailConnector
    {
        private readonly SessionData _sessionData;
        private readonly RestAPI _restAPI;

        public EmailConnector(
            SessionData sessionData,
            RestAPI restAPI)
        {
            _sessionData = sessionData;
            _restAPI = restAPI;
        }
        
        public async void Register(RegistrationData data)
        {
            if (data.Email != data.EmailConfirmation) return;
            if (data.Password != data.PasswordConfirmation) return;

            await _restAPI.Register(_sessionData.Token, data.Email, data.Password, data.ReferralCode ?? "");
        }

        public async void Forget(ForgetData data)
        {
            if (string.IsNullOrEmpty(data.Email)) return;
            
            await _restAPI.Recover(data.Email);
        }

        public async void Login(LoginData data)
        {
            var token = await _restAPI.RestoreByEmail(SystemInfo.deviceUniqueIdentifier, data.Email, data.Password);
            if (token != null)
            {
                _sessionData.SetToken(token);
            }
        }
    }

    public struct RegistrationData
    {
        public string Email;
        public string EmailConfirmation;
        public string Password;
        public string PasswordConfirmation;
        public string ReferralCode;

        public RegistrationData(string email, string emailConfirmation, string password, string passwordConfirmation, string referralCode)
        {
            Email = email;
            EmailConfirmation = emailConfirmation;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
            ReferralCode = referralCode;
        }
    }

    public struct ForgetData
    {
        public string Email;

        public ForgetData(string email)
        {
            Email = email;
        }
    }
    
    public struct LoginData
    {
        public string Email;
        public string Password;

        public LoginData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}