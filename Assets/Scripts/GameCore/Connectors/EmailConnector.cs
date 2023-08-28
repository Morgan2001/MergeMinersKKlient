using System.Threading.Tasks;
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
        
        public async Task<bool> Register(RegistrationData data)
        {
            if (data.Email != data.EmailConfirmation) return false;
            if (data.Password != data.PasswordConfirmation) return false;

            var result = await _restAPI.Register(_sessionData.Token, data.Email, data.Password, data.ReferralCode ?? "");
            if (result.ResultType != RestResultType.Success) return false;
            
            _sessionData.SetEmail(data.Email);
            return true;
        }

        public async void Forget(ForgetData data)
        {
            if (string.IsNullOrEmpty(data.Email)) return;
            
            var result = await _restAPI.Recover(data.Email);
            if (result.ResultType == RestResultType.Success)
            {
                _sessionData.SetEmail(data.Email);
            }
        }

        public async void Delete()
        {
            await _restAPI.Delete(SystemInfo.deviceUniqueIdentifier);
        }



        public async void Login(LoginData data)
        {
            var result = await _restAPI.RestoreByEmail(SystemInfo.deviceUniqueIdentifier, data.Email, data.Password);
            if (result.ResultType == RestResultType.Success)
            {
                _sessionData.SetToken(result.Result);
                _sessionData.SetEmail(data.Email);
            }
        }
        
        public void Logout()
        {
            _sessionData.SetEmail(null);
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