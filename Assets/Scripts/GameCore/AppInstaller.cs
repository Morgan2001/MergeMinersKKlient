﻿using Ads;
using IAP;
using UI.BottomPanel;
using UI.GameplayPanel;
using UI.GameplayPanel.MergePanel;
using UI.GameplayPanel.ShopPanel;
using UI.ShopScreen;
using UI.TopPanel;
using UI.Utils;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private SetOfMiningDeviceDatas _miningDeviceDatas;
        [SerializeField] private SetOfMiningDeviceBoxes _miningDeviceBoxes;
        [SerializeField] private SetOfLocations _locations;
        [SerializeField] private SetOfFlyingBonuses _flyingBonuses;
        [SerializeField] private SetOfWheelRewards _wheelRewards;
        [SerializeField] private SetOfUpgrades _upgrades;
        
        [SerializeField] private TopPanelSetup _topPanelSetup;
        [SerializeField] private MergePanelSetup _mergePanelSetup;
        [SerializeField] private ShopPanelSetup _shopPanelSetup;
        [SerializeField] private MinerShopPanelSetup _minerShopPanelSetup;
        [SerializeField] private BottomPanelSetup _bottomPanelSetup;
        
        [SerializeField] private DragHelper _dragHelper;
        
        [SerializeField] private IAPController _iapController;
        [SerializeField] private AdsController _adsController;

        public override void InstallBindings()
        {
            Container.BindInstance(_miningDeviceDatas);
            Container.BindInstance(_miningDeviceBoxes);
            Container.BindInstance(_locations);
            Container.BindInstance(_flyingBonuses);
            Container.BindInstance(_wheelRewards);
            Container.BindInstance(_upgrades);
            Container.BindInterfacesTo<ResourceHelper>().AsSingle();
            
            Container.BindInstance(_topPanelSetup);
            Container.BindInstance(_mergePanelSetup);
            Container.BindInstance(_shopPanelSetup);
            Container.BindInstance(_minerShopPanelSetup);
            Container.BindInstance(_bottomPanelSetup);
            Container.Bind<GameplayViewStorage>().AsSingle();
            Container.Bind<TabSwitcher>().AsSingle();
            
            Container.BindInstance(_dragHelper);
            
            Container.Bind<StoreListener>().AsSingle();
            Container.BindInstance(_iapController);
            Container.BindInstance(_adsController);
        }
    }
}