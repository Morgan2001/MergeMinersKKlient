using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Proxy.Connectors;
using Cysharp.Threading.Tasks;
using MergeMiner.Core.Events.Events;
using MergeMiner.Core.State.Enums;
using UI.Utils;
using UnityEngine;
using Utils;
using Utils.MVVM;
using Zenject;

namespace UI.GameplayPanel.MergePanel
{
    public class MergePanelSetup : MonoBehaviour
    {
        [SerializeField] private MergePanelView _mergePanelView;

        private MinerFieldConnector _minerFieldConnector;
        private IMinerResourceHelper _minerResourceHelper;
        private DragHelper _dragHelper;
        
        private MergePanelViewModel _mergePanelViewModel;

        private List<CellViewModel> _cells = new();
        private List<MinerViewModel> _miners = new();

        private Dictionary<MinerViewModel, int> _slotsByMiners = new();
        
        private ReactiveProperty<bool> _isDragging = new();

        private List<CellViewModel> _highlightedCells = new();

        [Inject]
        private void Setup(
            MinerFieldConnector minerFieldConnector, 
            IMinerResourceHelper minerResourceHelper,
            DragHelper dragHelper)
        {
            _minerFieldConnector = minerFieldConnector;
            _minerFieldConnector.ResizeEvent.Subscribe(OnResize).AddTo(_mergePanelView);
            _minerFieldConnector.AddMinerEvent.Subscribe(OnAddMiner).AddTo(_mergePanelView);
            _minerFieldConnector.MergeMinersEvent.Subscribe(OnMergeMiners).AddTo(_mergePanelView);
            _minerFieldConnector.SwapMinersEvent.Subscribe(OnSwapMiners).AddTo(_mergePanelView);
            _minerFieldConnector.RemoveMinerEvent.Subscribe(OnRemoveMiner).AddTo(_mergePanelView);

            _minerResourceHelper = minerResourceHelper;
            
            _mergePanelViewModel = new MergePanelViewModel();
            _mergePanelView.Bind(_mergePanelViewModel);

            _dragHelper = dragHelper;
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
                _highlightedCells.AddRange(highlightedMiners.Select(x => _cells[x]));
                
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

        private void OnResize(RelocateEvent gameEvent)
        {
            _mergePanelViewModel.SetSize(gameEvent.Width, gameEvent.Height);
            CreateCells(gameEvent.Total - _cells.Count);
            UpdateCells(gameEvent.Powered);
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
            _mergePanelViewModel.AddSlot(cell);
            
            var view = _mergePanelView.GetCell(cell);
            view.DropEvent.Subscribe(OnDrop).AddTo(view);
            
        }
        
        private void OnDrag(MinerView minerView)
        {
            _dragHelper.StartDrag(minerView);
        }

        private void OnDrop(CellView cellView)
        {
            _dragHelper.EndDrag();
            
            var minerView = _dragHelper.Current;
            if (!_minerFieldConnector.Drop(minerView.ViewModel.Id, cellView.ViewModel.Id))
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
        }

        private void ClearCells()
        {
            _cells.Clear();
            _mergePanelViewModel.Clear();
        }

        private void OnAddMiner(AddMinerData data)
        {
            AddMiner(data.Id, data.Name, data.Level, data.Slot, data.Source);
        }
        
        private void OnMergeMiners(MergeMinersData data)
        {
            foreach (var id in data.Merged)
            {
                RemoveMiner(id);
            }
            
            AddMiner(data.Id, data.Name, data.Level, data.Slot);
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

        private void AddMiner(string id, string name, int level, int slot, MinerSource source = MinerSource.None)
        {
            var normalIcon = _minerResourceHelper.GetNormalIconByName(name);
            var poweredIcon = _minerResourceHelper.GetPoweredIconByName(name);
            var boxIcon = _minerResourceHelper.GetBoxIconByType(source);
            var miner = new MinerViewModel(id, level, normalIcon, poweredIcon, boxIcon);
            _miners.Add(miner);
            _mergePanelViewModel.AddMiner(miner, slot);
            _slotsByMiners.Add(miner, slot);
            
            var view = _mergePanelView.GetMiner(miner);
            view.BeginDragEvent.Subscribe(OnDrag).AddTo(view);
            
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
            miner.Dispose();
        }
    }
}