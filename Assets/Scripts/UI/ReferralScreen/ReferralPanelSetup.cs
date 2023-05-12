using GameCore.Connectors;
using UnityEngine;
using UnityEngine.UI;
using Utils.Reactive;
using Zenject;

namespace UI.ReferralScreen
{
    public class ReferralPanelSetup : MonoBehaviour
    {
        [SerializeField] private Text _code;
        [SerializeField] private Button _copyButton;
        
        [SerializeField] private Text _referrals;
        [SerializeField] private Text _gems;

        private ReferralConnector _referralConnector;

        [Inject]
        private void Setup(
            ReferralConnector referralConnector)
        {
            _referralConnector = referralConnector;
            _referralConnector.UpdateInfoEvent.Subscribe(UpdateInfo);

            _copyButton.Subscribe(_referralConnector.Copy);
        }

        private void UpdateInfo(ReferralData data)
        {
            _code.text = data.Code;
            _referrals.text = data.Referrals.ToString();
            _gems.text = data.Gems.ToString();
        }
    }
}