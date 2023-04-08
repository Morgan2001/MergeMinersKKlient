﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;

namespace UI.GameplayPanel.ShopPanel
{
    public class ShopPanelView : View<ShopPanelViewModel>
    {
        [SerializeField] private HorizontalLayoutGroup _layout;
        [SerializeField] private MinerShopView _minerShopPrefab;

        private List<MinerShopView> _miners = new();

        protected override void BindInner(ShopPanelViewModel vm)
        {
            _vm.MinerAddedEvent.Subscribe(AddMiner).AddTo(this);
            _vm.ClearEvent.Subscribe(Clear).AddTo(this);
        }
        
        private void AddMiner(MinerShopViewModel minerShopViewModel)
        {
            var miner = Instantiate(_minerShopPrefab, _layout.transform);
            miner.Bind(minerShopViewModel);
            _miners.Add(miner);
        }

        private void Clear()
        {
            foreach (var minerView in _miners)
            {
                minerView.Dispose();
                Destroy(minerView.gameObject);
            }
            _miners.Clear();
        }
    }

    public class ShopPanelViewModel : ViewModel
    {
        private List<MinerShopViewModel> _miners = new();
        public IReadOnlyList<MinerShopViewModel> Miners => _miners;

        private ReactiveEvent<MinerShopViewModel> _minerAddedEvent = new();
        public IReactiveSubscription<MinerShopViewModel> MinerAddedEvent => _minerAddedEvent;
        
        public ReactiveEvent ClearEvent { get; } = new();
        
        public void AddMiner(MinerShopViewModel miner)
        {
            _miners.Add(miner);
            _minerAddedEvent.Trigger(miner);
        }

        public void Clear()
        {
            ClearEvent.Trigger();
            _miners.Clear();
        }
    }
}