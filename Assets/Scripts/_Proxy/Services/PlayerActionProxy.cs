using System;
using System.Threading.Tasks;
using _Proxy.Commands;
using _Proxy.Data;
using _Proxy.Events;
using _Proxy.Preloader;
using MergeMiner.Core.Bonuses.Services;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.Network.Data;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Data;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Services;
using MergeMiner.Core.State.Services.State;

namespace _Proxy.Services
{
    public class PlayerActionProxy
    {
        private readonly SessionData _sessionData;
        private readonly RestAPI _restAPI;
        private readonly PlayerActionService _playerActionService;
        private readonly GameCommandService _gameCommandService;
        private readonly BoxService _boxService;
        private readonly PlayerStateService _playerStateService;
        private readonly PlayerSlotsStateService _playerSlotsStateService;
        private readonly BonusApplierService _bonusApplierService;
        private readonly EventDispatcherService _eventDispatcherService;

        public PlayerActionProxy(
            SessionData sessionData,
            RestAPI restAPI,
            PlayerActionService playerActionService,
            GameCommandService gameCommandService,
            BoxService boxService,
            PlayerStateService playerStateService,
            PlayerSlotsStateService playerSlotsStateService,
            BonusApplierService bonusApplierService,
            EventDispatcherService eventDispatcherService)
        {
            _sessionData = sessionData;
            _restAPI = restAPI;
            _playerActionService = playerActionService;
            _gameCommandService = gameCommandService;
            _boxService = boxService;
            _playerStateService = playerStateService;
            _playerSlotsStateService = playerSlotsStateService;
            _bonusApplierService = bonusApplierService;
            _eventDispatcherService = eventDispatcherService;
        }

        private async void RestCall<T>(Func<string, Task<T>> call, Action<T> callback = null)
            where T : RestResponse
        {
            var response = await call.Invoke(_sessionData.Token);
            if (response.Success)
            {
                callback?.Invoke(response);
            }

            if (_playerStateService.SetMoney(_sessionData.Token, response.Money))
            {
                _eventDispatcherService.Dispatch(new ChangeMoneyEvent(_sessionData.Token, response.Money));
            }

            if (_playerStateService.SetGems(_sessionData.Token, response.Gems))
            {
                _eventDispatcherService.Dispatch(new ChangeGemsEvent(_sessionData.Token, response.Gems));
            }

            if (_playerSlotsStateService.SetLevel(_sessionData.Token, response.Location))
            {
                _eventDispatcherService.Dispatch(new UpdateSlotsLevelEvent(_sessionData.Token, response.Location));
            }
            
            _boxService.UpdateBoxTime(_sessionData.Token, response.BoxUnlockTime);

            if (response.AddedMiners != null)
            {
                foreach (var miner in response.AddedMiners)
                {
                    _gameCommandService.Process(new AddMinerCommand(_sessionData.Token, miner.Config, miner.Slot, miner.Source));
                }
            }
            
            if (response.AddedBoosts != null)
            {
                foreach (var boost in response.AddedBoosts)
                {
                    if (boost.Duration > 0)
                    {
                        _bonusApplierService.Apply(_sessionData.Token, boost.Type, boost.Duration, boost.Power);
                    }
                }
            }
        }

        public void SpawnBox()
        {
            RestCall(_restAPI.SpawnBox);
        }
        
        public void BuyMiner(int level, Currency currency)
        {
            RestCall(token => _restAPI.BuyMiner(token, level, currency));
        }
        
        public void MergeMiners(int slot1, int slot2)
        {
            _playerActionService.Process(new MergeMinersPlayerAction(_sessionData.Token, slot1, slot2));
            RestCall(token => _restAPI.MergeMiners(token, slot1, slot2));
        }
        
        public void RemoveMiner(int slot)
        {
            _playerActionService.Process(new RemoveMinerPlayerAction(_sessionData.Token, slot));
            RestCall(token => _restAPI.RemoveMiner(token, slot));
        }
        
        public void SwapMiners(int slot1, int slot2)
        {
            _playerActionService.Process(new SwapMinersPlayerAction(_sessionData.Token, slot1, slot2));
            RestCall(token => _restAPI.SwapMiners(token, slot1, slot2));
        }
        
        public void GetFreeGem()
        {
            _playerActionService.Process(new GetFreeGemPlayerAction(_sessionData.Token));
            RestCall(_restAPI.GetFreeGem);
        }
        
        public void Relocate()
        {
            _playerActionService.Process(new RelocatePlayerAction(_sessionData.Token));
            RestCall(_restAPI.Relocate);
        }
        
        public void SpeedUpBlueBox()
        {
            RestCall(_restAPI.SpeedUpBlueBox);
        }
        
        public void UseBonus(string id)
        {
            if (id == BonusNames.Wallet)
            {
                _playerActionService.Process(new UseBonusPlayerAction(_sessionData.Token, id));
            }
            RestCall(token => _restAPI.UseBonus(token, id));
        }
        
        public void SpinWheel(Currency currency)
        {
            RestCall(token => _restAPI.SpinWheel(token, currency), response =>
            {
                _eventDispatcherService.Dispatch(new SpinWheelEvent(_sessionData.Token, response.SpinResult!));
            });
        }

        public void MultiplyIncome()
        {
            RestCall(token => _restAPI.MultiplyIncome(token));
        }

        public void BuyUpgrade(string id)
        {
            RestCall(token => _restAPI.BuyUpgrade(token, id));
        }

        public void CollectMission(string id)
        {
            RestCall(token => _restAPI.CollectMission(token, id));
        }
        
        public void PurchaseTest(string id)
        {
            RestCall(token => _restAPI.PurchaseTest(token, id));
        }
        
        public void PurchaseTestSubscription()
        {
            RestCall(token => _restAPI.PurchaseTestSubscription(token));
        }
    }
}