using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.GameplayPanel.MergePanel
{
    public class CellView : View<CellViewModel>, IDropHandler
    {
        [SerializeField] private GameObject _filledBackground;
        [SerializeField] private GameObject _powered;
        [SerializeField] private GameObject _highlight;
        [SerializeField] private RawImage _matrix;
        [SerializeField] private Vector2 _speed;

        private ReactiveEvent<CellView> _dropEvent = new();
        public IReactiveSubscription<CellView> DropEvent => _dropEvent;

        private Vector2 _shift;

        protected override void BindInner(CellViewModel vm)
        {
            _vm.Filled.Bind(UpdateFilled).AddTo(this);
            _vm.Powered.Bind(UpdatePowered).AddTo(this);
            _vm.Highlight.Bind(UpdateHighlight).AddTo(this);
        }
        
        private void UpdateFilled(bool value)
        {
            _filledBackground.SetActive(value);
        }

        private void UpdatePowered(bool value)
        {
            _powered.SetActive(value);
        }
        
        private void UpdateHighlight(bool value)
        {
            _highlight.SetActive(value);
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            _dropEvent.Trigger(this);
        }

        private void Update()
        {
            if (!_vm.Powered.Value || !_vm.Filled.Value) return;
            
            _shift += Time.deltaTime * _speed;
            _shift.x %= 1;
            _shift.y %= 1;
            _matrix.uvRect = new Rect(_shift, Vector2.one);
        }
    }

    public class CellViewModel : ViewModel
    {
        public int Id { get; }
        
        private readonly ReactiveProperty<bool> _filled = new();
        public IReactiveProperty<bool> Filled => _filled;
        
        private readonly ReactiveProperty<bool> _powered = new();
        public IReactiveProperty<bool> Powered => _powered;
        
        private readonly ReactiveProperty<bool> _highlight = new();
        public IReactiveProperty<bool> Highlight => _highlight;

        public CellViewModel(int id)
        {
            Id = id;
        }

        public void SetFilled(bool value)
        {
            _filled.Set(value);
        }

        public void SetPowered(bool value)
        {
            _powered.Set(value);
        }
        
        public void SetHighlight(bool value)
        {
            _highlight.Set(value);
        }
    }
}