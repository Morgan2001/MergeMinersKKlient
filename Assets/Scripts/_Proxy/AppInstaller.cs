using UI.BottomPanel;
using UI.GameplayPanel;
using UI.GameplayPanel.MergePanel;
using UI.GameplayPanel.ShopPanel;
using UI.TopPanel;
using UI.Utils;
using UnityEngine;
using Zenject;

namespace _Proxy
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private SetOfMiningDeviceDatas _miningDeviceDatas;
        [SerializeField] private SetOfMiningDeviceBoxes _miningDeviceBoxes;
        
        [SerializeField] private TopPanelSetup _topPanelSetup;
        [SerializeField] private MergePanelSetup _mergePanelSetup;
        [SerializeField] private ShopPanelSetup _shopPanelSetup;
        [SerializeField] private BottomPanelSetup _bottomPanelSetup;
        
        [SerializeField] private DragHelper _dragHelper;

        public override void InstallBindings()
        {
            Container.BindInstance(_miningDeviceDatas);
            Container.BindInstance(_miningDeviceBoxes);
            Container.BindInterfacesTo<ResourceHelper>().AsSingle();
            
            Container.BindInstance(_topPanelSetup);
            Container.BindInstance(_mergePanelSetup);
            Container.BindInstance(_shopPanelSetup);
            Container.BindInstance(_bottomPanelSetup);
            Container.Bind<GameplayViewStorage>().AsSingle();
            
            Container.BindInstance(_dragHelper);
        }
    }
}