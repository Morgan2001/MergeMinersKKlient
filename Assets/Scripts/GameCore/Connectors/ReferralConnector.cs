using GameCore.Events;
using GameCore.Preloader;
using MergeMiner.Core.State.Services;
using UnityEngine;
using Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class ReferralConnector
    {
        private readonly SessionData _sessionData;
        private readonly EventSubscriptionService _eventSubscriptionService;
        
        private ReactiveEvent<ReferralData> _updateInfoEvent = new();
        public IReactiveSubscription<ReferralData> UpdateInfoEvent => _updateInfoEvent;

        public ReferralConnector(
            SessionData sessionData,
            EventSubscriptionService eventSubscriptionService)
        {
            _sessionData = sessionData;
            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInit);
        }

        private void OnInit(InitGameEvent gameEvent)
        {
            var state = _sessionData.GameState;
            _updateInfoEvent.Trigger(new ReferralData(state.ReferralCode, state.Referrals, state.ReferralRewards));
        }

        public void Copy()
        {
            GUIUtility.systemCopyBuffer = _sessionData.GameState.ReferralCode;
        }
    }

    public class ReferralData
    {
        public string Code { get; }
        public int Referrals { get; }
        public int Gems { get; }

        public ReferralData(string code, int referrals, int gems)
        {
            Code = code;
            Referrals = referrals;
            Gems = gems;
        }
    }
}