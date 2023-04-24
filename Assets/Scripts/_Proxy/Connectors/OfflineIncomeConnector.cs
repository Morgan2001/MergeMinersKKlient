﻿using _Proxy.Preloader;
using _Proxy.Services;

namespace _Proxy.Connectors
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
            if (multiply)
            {
                _playerActionProxy.MultiplyIncome();
            }
        }
    }
}