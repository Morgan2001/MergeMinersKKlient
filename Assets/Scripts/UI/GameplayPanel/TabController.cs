using UI.Utils;
using UnityEngine;
using Zenject;

namespace UI.GameplayPanel
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private GameObject _shop;
        [SerializeField] private GameObject _upgrades;
        [SerializeField] private GameObject _game;
        
        private TabSwitcher _tabSwitcher;
        
        [Inject]
        private void Setup(
            TabSwitcher tabSwitcher)
        {
            _tabSwitcher = tabSwitcher;

            _tabSwitcher.SwitchTabEvent.Subscribe(OnSwitchTab);
        }

        private void Awake()
        {
            OnSwitchTab(Tab.Game);
        }

        private void OnSwitchTab(Tab tab)
        {
            _shop.SetActive(tab == Tab.Shop);
            _upgrades.SetActive(tab == Tab.Upgrades);
            _game.SetActive(tab == Tab.Game);
        }
    }
}