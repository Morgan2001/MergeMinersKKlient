using System.Collections.Generic;
using System.Linq;
using GameCore.Connectors;
using GameCore.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Data;
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
        private AdsConnector _adsConnector;

        private List<BonusView> _bonuses = new();

        [Inject]
        private void Setup(
            BonusConnector bonusConnector,
            PopupsConnector popupsConnector,
            IResourceHelper resourceHelper,
            AdsConnector adsConnector)
        {
            _bonusConnector = bonusConnector;
            _popupsConnector = popupsConnector;
            _resourceHelper = resourceHelper;
            _adsConnector = adsConnector;

            _bonusConnector.AddBonusEvent.Subscribe(AddBonus);
            _bonusConnector.UseBonusEvent.Subscribe(UseBonus);
        }

        private void AddBonus(AddBonusData data)
        {
            var icon = _resourceHelper.GetBonusIconByType(data.Id);
            var viewModel = new BonusViewModel(icon);
            
            var view = Instantiate(_bonusPrefab, _container);
            view.ClickEvent.Subscribe(() =>
            {
                view.gameObject.SetActive(false);
                TryToUseBonus(data.Id);
            }).AddTo(view);
            
            view.Bind(viewModel);
            _bonuses.Add(view);

            Rotate(view);
            Move(view, data.Id);
        }

        private void TryToUseBonus(string id)
        {
            if (id == BonusNames.Wallet)
            {
                _bonusConnector.UseBonus(id);
            }
            else
            {
                _popupsConnector.ShowBonus(id, () =>
                {
                    _adsConnector.ShowRewarded(x =>
                    {
                        if (x)
                        {
                            _bonusConnector.UseBonus(id);
                        }
                    });
                });
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

        private async void Move(BonusView view, string id)
        {
            var config = _resourceHelper.GetBonusDataById(id);
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
            if (!_bonuses.Any()) return;
            
            var currentBonus = _bonuses.Last();

            if (data.BoostType == BoostType.Money)
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