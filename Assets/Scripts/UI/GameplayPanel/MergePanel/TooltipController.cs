using System.Linq;
using System.Threading;
using UI.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GameplayPanel.MergePanel
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private Image _tooltip;
        [SerializeField] private Transform _blueBox;
        [SerializeField] private float _tooltipDelay = 2.5f;

        private GameplayViewStorage _gameplayViewStorage;
        
        private CancellationTokenSource _cancellationTokenSource;

        private float _delay;

        [Inject]
        private void Setup(
            GameplayViewStorage gameplayViewStorage)
        {
            _gameplayViewStorage = gameplayViewStorage;
            _delay = _tooltipDelay;
        }
        
        private bool CheckLockedMiners()
        {
            var lockedMiner = _gameplayViewStorage.Miners
                .FirstOrDefault(x => !x.ViewModel.IsUnlocked.Value);
            if (lockedMiner == null) return false;
            
            AnimateTaps(lockedMiner.transform, 1);
            return true;
        }

        private bool CheckMergeMiners()
        {
            var miners = _gameplayViewStorage.Miners
                .Where(x => x.ViewModel.IsUnlocked.Value)
                .Reverse()
                .OrderByDescending(x => x.ViewModel.Level)
                .ToArray();
            
            for (int i = 0; i < miners.Length - 1; i++)
            {
                var miner = miners[i];
                var nextMiner = miners[i + 1];

                if (miner.ViewModel.Level != nextMiner.ViewModel.Level) continue;
                    
                AnimateMove(miner.transform, nextMiner.transform);
                return true;
            }

            return false;
        }
        
        private bool CheckPowerMiners()
        {
            var cells = _gameplayViewStorage.Cells.ToArray();
            var emptyPoweredCell = cells.FirstOrDefault(x => x.ViewModel.Powered.Value && !x.ViewModel.Filled.Value);

            if (emptyPoweredCell == null) return false;
            
            var miners = _gameplayViewStorage.Miners.ToArray();
            var notPoweredMiner = miners
                .Where(x => !x.ViewModel.IsPowered.Value)
                .OrderByDescending(x => x.ViewModel.Level)
                .ThenBy(x => x.transform.position.y)
                .ThenBy(x => x.transform.position.x)
                .FirstOrDefault();
            if (notPoweredMiner == null) return false;
            
            AnimateMove(notPoweredMiner.transform, emptyPoweredCell.transform);
            return true;
        }
        
        private void AnimateMove(Transform from, Transform to)
        {
            _cancellationTokenSource?.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();
            _delay = 30f;
            TooltipHelper.AnimateMove(_tooltip, from.position, to.position, () =>
            {
                _delay = _tooltipDelay;
                AnimateMove(from, to);
            }, _cancellationTokenSource.Token);
        }
        
        private void AnimateBlueBox()
        {
            AnimateTaps(_blueBox, 3);
        }
        
        private void AnimateTaps(Transform target, int count)
        {
            _cancellationTokenSource?.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();
            _delay = 3f;
            TooltipHelper.AnimateTap(_tooltip, target.position, count, null, _cancellationTokenSource.Token);
        }

        private void Update()
        {
            _delay -= Time.deltaTime;

            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                _delay = _tooltipDelay;
                
                if (_cancellationTokenSource == null) return;
                
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
            else if (_delay <= 0)
            {
                if (!CheckLockedMiners() && 
                    !CheckMergeMiners() && 
                    !CheckPowerMiners())
                {
                    AnimateBlueBox();
                }
            }
        }
    }
}