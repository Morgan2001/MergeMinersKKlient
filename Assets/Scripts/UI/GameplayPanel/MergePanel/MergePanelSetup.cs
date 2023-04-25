using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Connectors;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using MergeMiner.Core.State.Enums;
using UI.GameplayPanel.ShopPanel;
using UI.Utils;
using UnityEngine;
using Utils;
using Utils.MVVM;
using Utils.Reactive;
using Zenject;

namespace UI.GameplayPanel.MergePanel
{
    public class MergePanelSetup : MonoBehaviour
    {
        [SerializeField] private MergePanelView _mergePanelView;
        
        [SerializeField] private Transform _animationContainer;
        [SerializeField] private Transform _boxTransform;
        
        [SerializeField] private Transform _effectsContainer;
        [SerializeField] private UIParticle _dustParticlesPrefab;

        private MinerFieldConnector _minerFieldConnector;
        private PopupsConnector _popupsConnector;
        private IResourceHelper _resourceHelper;
        private GameplayViewStorage _gameplayViewStorage;
        private DragHelper _dragHelper;
        
        private MergePanelViewModel _mergePanelViewModel;
        private ShopPanelViewModel _shopPanelViewModel;

        private List<CellViewModel> _cells = new();
        private List<MinerViewModel> _miners = new();

        private Dictionary<MinerViewModel, int> _slotsByMiners = new();
        
        private ReactiveProperty<bool> _isDragging = new();

        private List<CellViewModel> _highlightedCells = new();

        private List<CellViewModel> _animatingCells = new();

        [Inject]
        private void Setup(
            MinerFieldConnector minerFieldConnector, 
            PopupsConnector popupsConnector,
            IResourceHelper resourceHelper,
            GameplayViewStorage gameplayViewStorage,
            DragHelper dragHelper)
        {
            _minerFieldConnector = minerFieldConnector;
            _popupsConnector = popupsConnector;
            _resourceHelper = resourceHelper;
            _gameplayViewStorage = gameplayViewStorage;
            _dragHelper = dragHelper;

            _mergePanelViewModel = new MergePanelViewModel();
            _mergePanelView.Bind(_mergePanelViewModel);
            
            _minerFieldConnector.ResizeEvent.Subscribe(OnResize).AddTo(_mergePanelView);
            _minerFieldConnector.AddMinerEvent.Subscribe(OnAddMiner).AddTo(_mergePanelView);
            _minerFieldConnector.MergeMinersEvent.Subscribe(OnMergeMiners).AddTo(_mergePanelView);
            _minerFieldConnector.SwapMinersEvent.Subscribe(OnSwapMiners).AddTo(_mergePanelView);
            _minerFieldConnector.RemoveMinerEvent.Subscribe(OnRemoveMiner).AddTo(_mergePanelView);

            _dragHelper.StartDragEvent.Subscribe(OnStartDrag);
            _dragHelper.EndDragEvent.Subscribe(OnEndDrag);

            _isDragging.Bind(OnDragging);
        }

        private void OnStartDrag()
        {
            _isDragging.Set(true);
        }
        
        private void OnEndDrag()
        {
            _isDragging.Set(false);
        }

        private void OnDragging(bool value)
        {
            if (value)
            {
                var minerView = _dragHelper.Current;
                var highlightedMiners = _slotsByMiners
                    .Where(x => x.Key.Level == minerView.ViewModel.Level && x.Key != minerView.ViewModel)
                    .Select(x => x.Value);
                _highlightedCells.AddRange(highlightedMiners.Select(x => _cells[x]).Where(x => !_animatingCells.Contains(x)));
                
                foreach (var cell in _highlightedCells)
                {
                    cell.SetHighlight(true);
                }
            }
            else
            {
                foreach (var cell in _highlightedCells)
                {
                    cell.SetHighlight(false);
                }
                _highlightedCells.Clear();
            }
        }

        private void OnResize(RelocateData data)
        {
            _mergePanelViewModel.SetSize(data.Width, data.Height);
            CreateCells(data.Total - _cells.Count);
            UpdateCells(data.Powered);
        }

        private void CreateCells(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddCell(_cells.Count);
            }
        }

        private void AddCell(int id)
        {
            var cell = new CellViewModel(id);
            _cells.Add(cell);
            _mergePanelViewModel.AddCell(cell);
            
            var view = _mergePanelView.GetCell(cell);
            view.DropEvent.Subscribe(OnDrop).AddTo(view);
            _gameplayViewStorage.AddCellView(id, view);
        }
        
        private void OnDrag(MinerView minerView)
        {
            _dragHelper.StartDrag(minerView);
        }

        private void OnDrop(CellView cellView)
        {
            _dragHelper.EndDrag();
            
            var minerView = _dragHelper.Current;
            if (_animatingCells.Contains(cellView.ViewModel) || !_minerFieldConnector.Drop(minerView.ViewModel.Id, cellView.ViewModel.Id))
            {
                var slot = _slotsByMiners[minerView.ViewModel];
                _mergePanelViewModel.ResetMiner(minerView.ViewModel, slot);
            }
        }

        private void UpdateCells(int powered)
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];
                cell.SetPowered(i < powered);
            }

            foreach (var entry in _slotsByMiners)
            {
                entry.Key.SetPowered(entry.Value < powered);
            }
        }

        private void ClearCells()
        {
            _cells.Clear();
            _mergePanelViewModel.Clear();
        }

        private void OnAddMiner(AddMinerData data)
        {
            AddMiner(data.Id, data.Level, data.Slot, data.Source);
            if (data.Source is MinerSource.Common or MinerSource.Random or MinerSource.RandomOpened)
            {
                AnimateNewBox(data, _boxTransform);
            } else if (data.Source == MinerSource.Shop)
            {
                AnimateNewBox(data, _gameplayViewStorage.GetMinerShopView(data.Name).transform);
            }
        }

        private void AnimateNewBox(AddMinerData data, Transform from)
        {
            var cellView = _gameplayViewStorage.GetCellView(data.Slot);
            var minerView = _gameplayViewStorage.GetMinerView(data.Id);
            _animatingCells.Add(cellView.ViewModel);
            AnimationHelper.AnimateNewBox(minerView.RectTransform, from.position, cellView.transform.position, _animationContainer,
            () =>
            {
                ShowDustParticles(minerView);
            },
            () =>
            {
                _animatingCells.Remove(cellView.ViewModel);
            });
        }

        private void ShowDustParticles(MinerView minerView)
        {
            var dust = Instantiate(_dustParticlesPrefab, _effectsContainer);
            dust.transform.position = minerView.transform.position;
            dust.Play();
            Destroy(dust.gameObject, 1f);
        }
        
        private void OnMergeMiners(MergeMinersData data)
        {
            RemoveMiner(data.Merged[0]);

            var id = data.Merged[1];

            var miner = _miners.First(x => x.Id == id);
            miner.Unlock();
            
            Transform GetTarget()
            {
                var view = _gameplayViewStorage.GetMinerView(id);
                return view.Icon.transform;
            }

            var cell = _cells[data.Slot];
            _animatingCells.Add(cell);

            AnimationHelper.AnimateMerge(GetTarget, () =>
            {
                RemoveMiner(id);
                id = data.Id;
                AddMiner(data.Id, data.Level, data.Slot);
                _animatingCells.Remove(cell);
            });
            
            HapticsHelper.MergeHaptics(data.MaxLevelIncreased);
        }
        
        private void OnSwapMiners(SwapMinersData data)
        {
            var miner1 = _slotsByMiners.FirstOrDefault(x => x.Value == data.Slot1).Key;
            var miner2 = _slotsByMiners.FirstOrDefault(x => x.Value == data.Slot2).Key;
            if (miner1 != null)
            {
                _slotsByMiners[miner1] = data.Slot2;
            }
            if (miner2 != null)
            {
                _slotsByMiners[miner2] = data.Slot1;
            }
            
            _mergePanelViewModel.SwapMiners(data.Slot1, data.Slot2);
        }
        
        private void OnRemoveMiner(RemoveMinerData data)
        {
            RemoveMiner(data.Miner);
        }

        private void AddMiner(string id, int level, int slot, MinerSource source = MinerSource.None)
        {
            var normalIcon = _resourceHelper.GetNormalIconByLevel(level);
            var poweredIcon = _resourceHelper.GetPoweredIconByLevel(level);
            var boxIcon = _resourceHelper.GetBoxIconByType(source);
            var miner = new MinerViewModel(id, level, normalIcon, poweredIcon, boxIcon);
            _miners.Add(miner);
            _mergePanelViewModel.AddMiner(miner, slot);
            _slotsByMiners.Add(miner, slot);
            
            var view = _mergePanelView.GetMiner(miner);
            view.BeginDragEvent.Subscribe(OnDrag).AddTo(view);
            _gameplayViewStorage.AddMinerView(id, view);
            
            miner.IsUnlocked.Bind(x =>
            {
                if (!x) return;
                var s = _slotsByMiners[miner];
                var cell = _cells[s];
                cell.SetFilled(true);
            }).AddTo(miner);

            if (source != MinerSource.None)
            {
                miner.ClickEvent.Subscribe(() =>
                {
                    if (source == MinerSource.Random)
                    {
                        _popupsConnector.RollRandom(level);
                    }
                    miner.Unlock();
                }).AddTo(miner);

                if (source == MinerSource.Common)
                {
                    UnlockAfter(miner, 5);
                }
            }
            else
            {
                miner.Unlock();
            }
        }

        private async void UnlockAfter(MinerViewModel miner, float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            miner.Unlock();
        }

        private void RemoveMiner(string id)
        {
            var miner = _miners.First(x => x.Id == id);
            var slot = _slotsByMiners[miner];
            _miners.Remove(miner);
            _mergePanelViewModel.RemoveMiner(miner, slot);
            _slotsByMiners.Remove(miner);
            _gameplayViewStorage.RemoveMinerView(id);
            miner.Dispose();
        }
    }
}