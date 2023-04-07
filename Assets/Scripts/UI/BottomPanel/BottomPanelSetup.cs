using _Proxy.Connectors;
using UI.Utils;
using UnityEngine;
using Utils;
using Utils.MVVM;
using Zenject;

namespace UI.BottomPanel
{
    public class BottomPanelSetup : MonoBehaviour
    {
        [SerializeField] private BlueBoxView _boxView;
        [SerializeField] private SocialView _socialView;
        [SerializeField] private TrashCanView _trashCanView;
        
        private BlueBoxViewModel _blueBoxViewModel;

        private PlayerBoxConnector _playerBoxConnector;
        private MinerFieldConnector _minerFieldConnector;
        private DragHelper _dragHelper;

        private TrashCanViewModel _trashCanViewModel;
        private SocialViewModel _socialViewModel;

        private ReactiveProperty<bool> _isDragging = new();

        [Inject]
        private void Setup(PlayerBoxConnector playerBoxConnector, MinerFieldConnector minerFieldConnector, DragHelper dragHelper)
        {
            _playerBoxConnector = playerBoxConnector;
            _playerBoxConnector.BoxProgress.Subscribe(OnBoxProgressUpdate).AddTo(_boxView);

            _minerFieldConnector = minerFieldConnector;

            _dragHelper = dragHelper;
            _dragHelper.StartDragEvent.Subscribe(OnStartDrag);
            _dragHelper.EndDragEvent.Subscribe(OnEndDrag);

            _blueBoxViewModel = new BlueBoxViewModel();
            _boxView.Bind(_blueBoxViewModel);
            
            _blueBoxViewModel.ClickEvent.Subscribe(OnBlueBoxClick).AddTo(_boxView);

            _trashCanViewModel = new TrashCanViewModel();
            _trashCanView.Bind(_trashCanViewModel);
            _trashCanView.DropEvent.Subscribe(OnDrop).AddTo(_trashCanView);

            _socialViewModel = new SocialViewModel();
            _socialView.Bind(_socialViewModel);

            _isDragging.Bind(x =>
            {
                _socialViewModel.SetShown(!x);
                _trashCanViewModel.SetShown(x);
            });
        }

        private void OnStartDrag()
        {
            _isDragging.Set(true);
        }
        
        private void OnEndDrag()
        {
            _isDragging.Set(false);
        }
        
        private void OnDrop()
        {
            var minerView = _dragHelper.Current;
            _minerFieldConnector.Remove(minerView.ViewModel.Id);
        }

        private void OnBlueBoxClick()
        {
            _playerBoxConnector.SpeedUp();
        }

        private void OnBoxProgressUpdate(float value)
        {
            _blueBoxViewModel.SetProgress(value);
        }
    }
}