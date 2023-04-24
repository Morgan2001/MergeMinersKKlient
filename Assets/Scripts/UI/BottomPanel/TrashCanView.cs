using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.BottomPanel
{
    public class TrashCanView : View<TrashCanViewModel>, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image; 
        [SerializeField] private Sprite _normal;
        [SerializeField] private Sprite _highlighted;
        
        private ReactiveEvent _dropEvent = new();
        public IReactiveSubscription DropEvent => _dropEvent;
        
        protected override void BindInner(TrashCanViewModel vm)
        {
            _vm.IsShown.Bind(OnShownUpdate).AddTo(this);
        }

        private void OnShownUpdate(bool value)
        {
            gameObject.SetActive(value);
        }

        private void SetHighlighted(bool value)
        {
            _image.sprite = value ? _highlighted : _normal;
        }

        public void OnDrop(PointerEventData eventData)
        {
            _dropEvent.Trigger();
            SetHighlighted(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetHighlighted(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetHighlighted(false);
        }
    }

    public class TrashCanViewModel : ViewModel
    {
        private readonly ReactiveProperty<bool> _isShown = new();
        public IReactiveProperty<bool> IsShown => _isShown;

        public void SetShown(bool value)
        {
            _isShown.Set(value);
        }
    }
}