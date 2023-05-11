using System.Collections.Generic;
using GameCore.Connectors;
using I2.Loc;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.MissionsScreen
{
    public class MissionsPanelSetup : MonoBehaviour
    {
        [SerializeField] private MissionView _missionPrefab;
        [SerializeField] private Transform _container;

        private MissionsConnector _missionsConnector;

        private Dictionary<string, MissionViewModel> _missions = new();

        [Inject]
        private void Setup(
            MissionsConnector missionsConnector)
        {
            _missionsConnector = missionsConnector;
            
            _missionsConnector.AddMissionEvent.Subscribe(OnAddMission);
            _missionsConnector.AddStatsEvent.Subscribe(OnAddStats);
        }

        private void OnAddMission(AddMissionData data)
        {
            var description = LocalizationManager.GetTranslation("mission_" + (int) data.Type);
            description = string.Format(description, data.Value, data.Level);
            var viewModel = new MissionViewModel(data.Id, description, data.Value);
            _missions.Add(data.Id, viewModel);
        
            var view = Instantiate(_missionPrefab, _container);
            view.Bind(viewModel);
        
            view.CollectEvent.Subscribe(id =>
            {
                _missionsConnector.Collect(id);
                Destroy(view.gameObject);
            }).AddTo(view);
        }

        private void OnAddStats(AddStatsData data)
        {
            if (!_missions.ContainsKey(data.Id)) return;
            _missions[data.Id].SetProgress(data.Value);
        }
    }
}