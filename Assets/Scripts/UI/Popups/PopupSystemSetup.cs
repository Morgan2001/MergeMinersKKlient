using _Proxy.Connectors;
using UI.Utils;
using UnityEngine;
using Zenject;

namespace UI.Popups
{
    public class PopupSystemSetup : MonoBehaviour
    {
        [SerializeField] private NewMinerPopup _newMinerPopup;

        private PopupsConnector _popupsConnector;
        private IMinerResourceHelper _resourceHelper;

        [Inject]
        private void Setup(
            PopupsConnector popupsConnector,
            IMinerResourceHelper resourceHelper)
        {
            _popupsConnector = popupsConnector;
            _resourceHelper = resourceHelper;

            _popupsConnector.NewMinerPopupEvent.Subscribe(OnNewMiner);
        }

        private void OnNewMiner(NewMinerPopupData data)
        {
            var icon = _resourceHelper.GetNormalIconByName(data.Config);
            var previousIcon = _resourceHelper.GetNormalIconByName(data.PreviousConfig);
            var viewModel = new NewMinerPopupViewModel(data.Config, data.Level, data.Income, icon, previousIcon);
            _newMinerPopup.Show(viewModel);
        }
    }
}