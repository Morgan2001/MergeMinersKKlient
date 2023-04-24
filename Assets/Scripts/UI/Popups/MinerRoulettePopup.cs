using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.Popups
{
    public class MinerRoulettePopup : Popup<MinerRoulettePopupViewModel>
    {
        [SerializeField] private RectTransform _spinner;
        [SerializeField] private Vector3 _spinPosition;

        [SerializeField] private RouletteCellView _winCell;
        [SerializeField] private List<RouletteCellView> _anotherCells;

        private ReactiveEvent _spinEvent = new();
        public IReactiveSubscription SpinEvent => _spinEvent;

        protected override void Create()
        {
            _winCell.Bind(new RouletteCellViewModel());
            _anotherCells.ForEach(x => x.Bind(new RouletteCellViewModel()));
        }

        protected override void BindInner(MinerRoulettePopupViewModel vm)
        {
            UpdateCell(_winCell.ViewModel, _vm.Miner);
            
            foreach (var cell in _anotherCells)
            {
                UpdateCell(cell.ViewModel, _vm.Random());
            }

            Spin();
        }

        private void UpdateCell(RouletteCellViewModel viewModel, RouletteMinerInfo info)
        {
            viewModel.SetLevel(info.Level);
            viewModel.SetIcon(info.Icon);
        }
        
        private async void Spin()
        {
            await _spinner.DOAnchorPos(Vector2.zero, 1).SetEase(Ease.OutQuad).From(_spinPosition).Play().ToUniTask();

            await UniTask.Delay(1000);
            
            _spinEvent.Trigger();
        }
    }

    public class MinerRoulettePopupViewModel : ViewModel
    {
        private RouletteMinerInfo _miner;
        public RouletteMinerInfo Miner => _miner;
        
        private Func<RouletteMinerInfo> _random;
        public Func<RouletteMinerInfo> Random => _random;

        public MinerRoulettePopupViewModel(RouletteMinerInfo miner, Func<RouletteMinerInfo> random)
        {
            _miner = miner;
            _random = random;
        }
    }

    public struct RouletteMinerInfo
    {
        public int Level { get; }
        public Sprite Icon { get; }

        public RouletteMinerInfo(int level, Sprite icon)
        {
            Level = level;
            Icon = icon;
        }
    }
}