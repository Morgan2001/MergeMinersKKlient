using System.Collections.Generic;
using System.Linq;
using _Proxy.Connectors;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MergeMiner.Core.State.Config;
using UI.Utils;
using UnityEngine;
using Utils.MVVM;
using Zenject;

namespace UI.GameplayPanel.FlyingBonuses
{
    public class FlyingBonusesSetup : MonoBehaviour
    {
        [SerializeField] private BonusView _bonusPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private MoneyBonusRewardView _moneyBonusRewardPrefab;
        [SerializeField] private FlyingBonusPanelMarker _leftMarker;
        [SerializeField] private FlyingBonusPanelMarker _rightMarker;

        private BonusConnector _bonusConnector;
        private PopupsConnector _popupsConnector;
        private IResourceHelper _resourceHelper;

        private List<BonusView> _bonuses = new();

        [Inject]
        private void Setup(
            BonusConnector bonusConnector,
            PopupsConnector popupsConnector,
            IResourceHelper resourceHelper)
        {
            _bonusConnector = bonusConnector;
            _popupsConnector = popupsConnector;
            _resourceHelper = resourceHelper;

            _bonusConnector.AddBonusEvent.Subscribe(AddBonus);
            _bonusConnector.UseBonusEvent.Subscribe(UseBonus);
        }

        private void AddBonus(AddBonusData data)
        {
            var icon = _resourceHelper.GetBonusIconByType(data.BonusType);
            var viewModel = new BonusViewModel(icon);
            
            var view = Instantiate(_bonusPrefab, _container);
            view.ClickEvent.Subscribe(() =>
            {
                view.gameObject.SetActive(false);
                TryToUseBonus(data.BonusType);
            }).AddTo(view);
            
            view.Bind(viewModel);
            _bonuses.Add(view);

            Rotate(view);
            Move(view, data.BonusType);
        }

        private void TryToUseBonus(BonusType bonusType)
        {
            if (bonusType == BonusType.Money)
            {
                _bonusConnector.UseBonus(bonusType);
            }
            else
            {
                _popupsConnector.ShowBonus(bonusType, () => _bonusConnector.UseBonus(bonusType));
            }
        }

        private async void Rotate(BonusView view)
        {
            await view.transform
                .DORotate(new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(int.MaxValue)
                .SetLink(view.gameObject)
                .Play()
                .ToUniTask();
        }

        private async void Move(BonusView view, BonusType bonusType)
        {
            var config = _resourceHelper.GetBonusDataByType(bonusType);
            var bounces = config.NumOfBouncesToDestroy;
            var timeToFly = config.TimeToFLyBetweenSides;
            var onTheLeft = Random.value < 0.5f;
            
            var target = onTheLeft ? _leftMarker.GetRandom() : _rightMarker.GetRandom();
            view.transform.position = target;
            
            while (bounces > 0 && _bonuses.Contains(view))
            {
                onTheLeft = !onTheLeft;
                target = onTheLeft ? _leftMarker.GetRandom() : _rightMarker.GetRandom();
                await view.transform.DOMove(target, timeToFly).SetEase(Ease.Linear).SetLink(view.gameObject).Play().ToUniTask();
                bounces--;
            }
            
            RemoveBonus(view);
        }

        private void UseBonus(UseBonusData data)
        {
            var currentBonus = _bonuses.Last();

            if (data.BonusType == BonusType.Money)
            {
                var viewModel = new MoneyBonusRewardViewModel(data.Value);
                var view = Instantiate(_moneyBonusRewardPrefab, _container);
                view.transform.position = currentBonus.transform.position;
                view.Bind(viewModel);
                Destroy(view.gameObject, 3);
            }

            RemoveBonus(currentBonus);
        }

        private void RemoveBonus(BonusView view)
        {
            if (!_bonuses.Contains(view)) return;
            
            _bonuses.Remove(view);
            Destroy(view.gameObject);
        }
    }
}