using System.Collections.Generic;
using System.Linq;
using GameCore.Events;
using GameCore.Preloader;
using GameCore.Services;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
using MergeMiner.Core.State.Events;
using MergeMiner.Core.State.Repository;
using MergeMiner.Core.State.Services;
using Utils;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class MissionsConnector
    {
        private readonly SessionData _sessionData;
        private readonly PlayerActionProxy _playerActionProxy;
        private readonly MissionsConfig _missionsConfig;
        private readonly PlayerMissionsRepository _playerMissionsRepository;
        private readonly EventSubscriptionService _eventSubscriptionService;
        
        private ReactiveEvent<AddMissionData> _addMissionEvent = new();
        public IReactiveSubscription<AddMissionData> AddMissionEvent => _addMissionEvent;

        private ReactiveEvent<AddStatsData> _addStatsEvent = new();
        public IReactiveSubscription<AddStatsData> AddStatsEvent => _addStatsEvent;

        public MissionsConnector(
            SessionData sessionData,
            PlayerActionProxy playerActionProxy,
            MissionsConfig missionsConfig,
            PlayerMissionsRepository playerMissionsRepository,
            EventSubscriptionService eventSubscriptionService)
        {
            _sessionData = sessionData;
            _playerActionProxy = playerActionProxy;
            _missionsConfig = missionsConfig;
            _playerMissionsRepository = playerMissionsRepository;

            _eventSubscriptionService = eventSubscriptionService;
            _eventSubscriptionService.Subscribe<InitGameEvent>(OnInitGame);
            _eventSubscriptionService.Subscribe<AddStatsEvent>(OnAddStats);
        }

        private void OnInitGame(InitGameEvent gameEvent)
        {
            var playerMissions = _playerMissionsRepository.Get(_sessionData.Token);
            var filter = _missionsConfig.GetAll().Where(x => !playerMissions.Collected.Contains(x.Id));
            foreach (var mission in filter)
            {
                _addMissionEvent.Trigger(new AddMissionData(mission.Id, mission.Type, mission.Level, mission.Value, mission.Reward, mission.Currency));
            }
            
            foreach (var entry in playerMissions.Stats)
            {
                _addStatsEvent.Trigger(new AddStatsData(entry.Key, entry.Value));
            }
        }

        public void Collect(string id)
        {
            _playerActionProxy.CollectMission(id);
        }

        private void OnAddStats(AddStatsEvent gameEvent)
        {
            _addStatsEvent.Trigger(new AddStatsData(gameEvent.Id, gameEvent.Value));
        }
    }

    public class AddMissionData
    {
        public string Id { get; }
        public MissionType Type { get; }
        public int Level { get; }
        public int Value { get; }
        public int Reward { get; }
        public Currency Currency { get; }

        public AddMissionData(string id, MissionType type, int level, int value, int reward, Currency currency)
        {
            Id = id;
            Type = type;
            Level = level;
            Value = value;
            Reward = reward;
            Currency = currency;
        }
    }

    public class AddStatsData
    {
        public string Id { get; }
        public int Value { get; }

        public AddStatsData(string id, int value)
        {
            Id = id;
            Value = value;
        }
    }
}