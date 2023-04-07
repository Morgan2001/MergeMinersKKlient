using System;
using UI.GameplayPanel.MergePanel;
using UnityEngine;
using Utils;

namespace UI.Utils
{
    public class DragHelper : MonoBehaviour
    {
        private MinerView _current;
        private Transform _parent;

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
            
            _current.DragEvent.Subscribe(OnDrag);
            
            _startDragEvent.Trigger();
            enabled = true;
        }

        public void EndDrag()
        {
            if (!enabled) return;
            
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
            _current.transform.position = Input.mousePosition;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }
}