using System.Collections.Generic;
using UI.GameplayPanel.MergePanel;
using UI.GameplayPanel.ShopPanel;

namespace UI.GameplayPanel
{
    public class GameplayViewStorage
    {
        private Dictionary<int, CellView> _cellViews = new();
        private Dictionary<string, MinerView> _minerViews = new();
        private Dictionary<string, MinerShopView> _minerShopViews = new();

        public CellView GetCellView(int id)
        {
            return _cellViews[id];
        }
        
        public MinerView GetMinerView(string id)
        {
            return _minerViews[id];
        }
        
        public MinerShopView GetMinerShopView(string id)
        {
            return _minerShopViews[id];
        }

        public void AddCellView(int id, CellView view)
        {
            _cellViews.Add(id, view);
        }
        
        public void AddMinerView(string id, MinerView view)
        {
            _minerViews.Add(id, view);
        }
        
        public void AddMinerShopView(string id, MinerShopView view)
        {
            _minerShopViews.Add(id, view);
        }
        
        public void RemoveCellView(int id)
        {
            _cellViews.Remove(id);
        }
        
        public void RemoveMinerView(string id)
        {
            _minerViews.Remove(id);
        }
        
        public void RemoveMinerShopView(string id)
        {
            _minerShopViews.Remove(id);
        }

        public void Clear()
        {
            _cellViews.Clear();
            _minerViews.Clear();
            _minerShopViews.Clear();
        }
    }
}