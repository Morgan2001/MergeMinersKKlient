﻿using System;
using System.Threading.Tasks;
using _Proxy.Commands;
using _Proxy.Preloader;
using MergeMiner.Core.Commands.Services;
using MergeMiner.Core.Network.Data;
using MergeMiner.Core.PlayerActions.Actions;
using MergeMiner.Core.PlayerActions.Services;
using MergeMiner.Core.State.Config;
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
        private readonly EventDispatcherService _eventDispatcherService;

        public PlayerActionProxy(
            SessionData sessionData,
            RestAPI restAPI,
            PlayerActionService playerActionService,
            GameCommandService gameCommandService,
            BoxService boxService,
            PlayerStateService playerStateService,
            PlayerSlotsStateService playerSlotsStateService,
            EventDispatcherService eventDispatcherService)
        {
            _sessionData = sessionData;
            _restAPI = restAPI;
            _playerActionService = playerActionService;
            _gameCommandService = gameCommandService;
            _boxService = boxService;
            _playerStateService = playerStateService;
            _playerSlotsStateService = playerSlotsStateService;
            _eventDispatcherService = eventDispatcherService;
        }

        private async void RestCall(Func<string, Task<RestResponse>> call, Action<RestResponse> callback = null)
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
        
        public void UseBonus(BonusType bonusType)
        {
            if (bonusType != BonusType.Miners)
            {
                _playerActionService.Process(new UseBonusPlayerAction(_sessionData.Token, bonusType));
            }
            RestCall(token => _restAPI.UseBonus(token, bonusType));
        }
    }
}