using GameCore.Preloader;
using GameCore.Services;

namespace GameCore.Connectors
{
    public class OfflineIncomeConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerActionProxy _playerActionProxy;

        public OfflineIncomeConnector(
            SessionData sessionData,
            PlayerActionProxy playerActionProxy)
        {
            _sessionData = sessionData;
            _playerActionProxy = playerActionProxy;
        }
        
        public void MultiplyIncome(bool multiply)
        {
            _sessionData.Start();
            _playerActionProxy.GetOfflineIncome(multiply);
        }
    }
}