using UI.GameplayPanel.MergePanel;
using UnityEngine;
using Utils;
using Utils.Reactive;

namespace UI.Utils
{
    public class DragHelper : MonoBehaviour
    {
        [SerializeField] private float _scaleFactor = 1.25f;
        [SerializeField] private float _lerpFactor = 20f;
        
        private MinerView _current;
        private Transform _parent;

        private Vector3 _targetScale;
        private Vector3 _target;

        public MinerView Current => _current;

        private ReactiveEvent _startDragEvent = new();
        public IReactiveSubscription StartDragEvent => _startDragEvent;
        
        private ReactiveEvent _endDragEvent = new();
        public IReactiveSubscription EndDragEvent => _endDragEvent;

        private void Awake()
        {
            enabled = false;
        }

        public void StartDrag(MinerView current)
        {
            _current = current;
            _parent = _current.transform.parent;
            _current.transform.SetParent(transform);
            _targetScale = _scaleFactor * Vector3.one;
            
            _current.DragEvent.Subscribe(OnDrag);
            
            _startDragEvent.Trigger();
            enabled = true;
        }

        public void EndDrag()
        {
            if (!enabled) return;

            _current.transform.localScale = Vector3.one;
            
            if (_current.transform.parent == transform)
            {
                _current.transform.SetParent(_parent);
                _current.transform.localPosition = Vector3.zero;
            }
            
            _current.DragEvent.Unsubscribe(OnDrag);
            
            _endDragEvent.Trigger();
            enabled = false;
        }

        private void OnDrag()
        {
            if (!enabled) return;

            _target = Input.mousePosition;
        }

        private void Update()
        {
            if (!enabled) return;
            
            _current.transform.localScale = Vector3.Lerp(_current.transform.localScale, _targetScale, Time.deltaTime * _lerpFactor);
            _current.transform.position = Vector3.Lerp(_current.transform.position, _target, Time.deltaTime * _lerpFactor);
            
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }
}