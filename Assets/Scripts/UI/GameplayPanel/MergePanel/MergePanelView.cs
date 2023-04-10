using System.Collections.Generic;
using UI.Utils;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.GameplayPanel.MergePanel
{
    public class MergePanelView : View<MergePanelViewModel>
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private MinerView _minerPrefab;

        private List<CellView> _cells = new();
        private Dictionary<string, MinerView> _miners = new();

        protected override void BindInner(MergePanelViewModel vm)
        {
            _vm.Size.Bind(UpdateGrid).AddTo(this);
            _vm.CellsCount.Bind(Resize).AddTo(this);
            _vm.MinerAddedEvent.Subscribe(AddMiner).AddTo(this);
            _vm.MinerMovedEvent.Subscribe(MoveMiner).AddTo(this);
            _vm.MinerResetEvent.Subscribe(ResetMiner).AddTo(this);
            _vm.MinerRemovedEvent.Subscribe(RemoveMiner).AddTo(this);
            _vm.ClearEvent.Subscribe(Clear).AddTo(this);
        }
        
        public CellView GetCell(CellViewModel cell)
        {
            return _cells[cell.Id];
        }

        public MinerView GetMiner(MinerViewModel miner)
        {
            return _miners[miner.Id];
        }

        private void UpdateGrid(FieldSize size)
        {
            var cellSize = SizeHelper.CalculateCellSize(_grid, _rectTransform.rect, size);
            _grid.cellSize = new Vector2(cellSize, cellSize);
            _grid.constraintCount = size.Width;
            
            foreach (var miner in _miners.Values)
            {
                miner.ViewModel.SetSize(_grid.cellSize.x);
            }
        }
        
        private void Resize(int count)
        {
            for (int i = _cells.Count; i < count; i++)
            {
                var cell = Instantiate(_cellPrefab, _grid.transform);
                cell.Bind(_vm.Cells[i]);
                _cells.Add(cell);
            }
        }
        
        private void AddMiner(MinerViewModel minerViewModel, int slot)
        {
            minerViewModel.SetSize(_grid.cellSize.x);
            var miner = Instantiate(_minerPrefab, _cells[slot].transform);
            miner.Bind(minerViewModel);
            _miners.Add(minerViewModel.Id, miner);
        }
        
        private void MoveMiner(MinerViewModel minerViewModel, int slot)
        {
            var miner = _miners[minerViewModel.Id];
            miner.transform.SetParent(_cells[slot].transform);
            miner.transform.localPosition = Vector3.zero;
        }
        
        private void ResetMiner(MinerViewModel minerViewModel, int slot)
        {
            var miner = _miners[minerViewModel.Id];
            miner.transform.SetParent(_cells[slot].transform);
            miner.transform.localPosition = Vector3.zero;
        }
        
        private void RemoveMiner(MinerViewModel minerViewModel)
        {
            var miner = _miners[minerViewModel.Id]; 
            _miners.Remove(minerViewModel.Id);
            Destroy(miner.gameObject);
        }

        private void Clear()
        {
            foreach (var cellView in _cells)
            {
                cellView.Dispose();
                Destroy(cellView.gameObject);
            }
            _cells.Clear();
            
            foreach (var minerView in _miners.Values)
            {
                minerView.Dispose();
                Destroy(minerView.gameObject);
            }
            _miners.Clear();
        }
    }

    public class MergePanelViewModel : ViewModel
    {
        private readonly ReactiveProperty<FieldSize> _size = new();
        public IReactiveProperty<FieldSize> Size => _size;

        private readonly ReactiveList<CellViewModel> _cells = new();
        public IReadOnlyList<CellViewModel> Cells => _cells;
        public IReactiveProperty<int> CellsCount => _cells;
        
        private readonly List<MinerViewModel> _miners = new();
        public IReadOnlyList<MinerViewModel> Miners => _miners;

        private readonly ReactiveEvent<MinerViewModel, int> _minerAddedEvent = new();
        public IReactiveSubscription<MinerViewModel, int> MinerAddedEvent => _minerAddedEvent;
        
        private readonly ReactiveEvent<MinerViewModel, int> _minerMovedEvent = new();
        public IReactiveSubscription<MinerViewModel, int> MinerMovedEvent => _minerMovedEvent;
        
        private readonly ReactiveEvent<MinerViewModel, int> _minerResetEvent = new();
        public IReactiveSubscription<MinerViewModel, int> MinerResetEvent => _minerResetEvent;
        
        private readonly ReactiveEvent<MinerViewModel> _minerRemovedEvent = new();
        public IReactiveSubscription<MinerViewModel> MinerRemovedEvent => _minerRemovedEvent;

        public ReactiveEvent ClearEvent { get; } = new();

        public void SetSize(int width, int height)
        {
            _size.Set(new FieldSize(width, height));
        }

        public void AddCell(CellViewModel cell)
        {
            _cells.Add(cell);
            _miners.Add(null);
        }
        
        public void AddMiner(MinerViewModel miner, int slot)
        {
            _miners[slot] = miner;
            _minerAddedEvent.Trigger(miner, slot);
            
            var cell = _cells[slot];
            miner.SetPowered(cell.Powered.Value);
        }

        public void MoveMiner(MinerViewModel miner, int slot)
        {
            _miners[slot] = miner;
            if (miner != null)
            {
                _minerMovedEvent.Trigger(miner, slot);
                
                var cell = _cells[slot];
                miner.SetPowered(cell.Powered.Value);
            }
            _cells[slot].SetFilled(miner != null);
        }
        
        public void SwapMiners(int slot1, int slot2)
        {
            var miner1 = _miners[slot1];
            var miner2 = _miners[slot2];
            MoveMiner(miner1, slot2);
            MoveMiner(miner2, slot1);
        }
        
        public void ResetMiner(MinerViewModel miner, int slot)
        {
            _minerResetEvent.Trigger(miner, slot);
        }

        public void RemoveMiner(MinerViewModel miner, int slot)
        {
            _miners[slot] = null;
            _cells[slot].SetFilled(false);
            _minerRemovedEvent.Trigger(miner);
        }

        public void Clear()
        {
            ClearEvent.Trigger();
            _cells.Clear();
            _miners.Clear();
        }
    }

    public struct FieldSize
    {
        public int Width { get; }
        public int Height { get; }

        public FieldSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}