using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.GameplayPanel.FlyingBonuses
{
    public class BonusView : View<BonusViewModel>, IPointerDownHandler
    {
        [SerializeField] private Image _icon;

        private ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;
        
        protected override void BindInner(BonusViewModel vm)
        {
            _icon.sprite = _vm.Icon;
        }

        private void UpdatePosition(Vector2 value)
        {
            transform.position = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _clickEvent.Trigger();
        }
    }

    public class BonusViewModel : ViewModel
    {
        public Sprite Icon { get; }

        public BonusViewModel(Sprite icon)
        {
            Icon = icon;
        }
    }
}