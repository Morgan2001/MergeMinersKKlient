﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Utils.MVVM;
using Utils.Reactive;

namespace UI.GameplayPanel.MergePanel
{
    public class MinerView : View<MinerViewModel>, IPointerDownHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;
        
        [SerializeField] private GameObject _commonState;
        [SerializeField] private GameObject _boxState;
        [SerializeField] private ShakeAnimation _shakeAnimation;
        
        [SerializeField] private Image _icon;
        public Image Icon => _icon;
        
        [SerializeField] private Text _level;
        [SerializeField] private Image _box;
        
        private ReactiveEvent<MinerView> _beginDragEvent = new();
        public IReactiveSubscription<MinerView> BeginDragEvent => _beginDragEvent;
        
        private ReactiveEvent _dragEvent = new();
        public IReactiveSubscription DragEvent => _dragEvent;
        
        protected override void BindInner(MinerViewModel vm)
        {
            _box.sprite = _vm.BoxIcon;
            
            _vm.IsMaxLevel.Bind(UpdateLevel).AddTo(this);
            _vm.IsUnlocked.Bind(UpdateUnlockedState).AddTo(this);
            _vm.IsPowered.Bind(UpdatePoweredState).AddTo(this);
            _vm.Size.Bind(UpdateSize).AddTo(this);
        }

        private void UpdateLevel(bool value)
        {
            _level.text = value ? "MAX" : _vm.Level.ToString();
        }

        private void UpdateSize(float value)
        {
            _rectTransform.sizeDelta = new Vector2(value, value);
        }

        private void UpdateUnlockedState(bool value)
        {
            _commonState.SetActive(value);
            _boxState.SetActive(!value);
        }
        
        private void UpdatePoweredState(bool value)
        {
            _icon.sprite = value ? _vm.PoweredIcon : _vm.NormalIcon;

            if (value)
            {
                _shakeAnimation.StartShaking();
            }
            else
            {
                _shakeAnimation.StopShaking();
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _vm.PointerDown();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _beginDragEvent.Trigger(this);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            _dragEvent.Trigger();
        }
    }

    public class MinerViewModel : ViewModel
    {
        public string Id { get; }
        
        public int Level { get; }
        public Sprite NormalIcon { get; }
        public Sprite PoweredIcon { get; }
        public Sprite BoxIcon { get; }
        
        private ReactiveProperty<bool> _isMaxLevel = new();
        public IReactiveProperty<bool> IsMaxLevel => _isMaxLevel;
        
        private ReactiveProperty<float> _size = new();
        public IReactiveProperty<float> Size => _size;
        
        private ReactiveProperty<bool> _isPowered = new();
        public IReactiveProperty<bool> IsPowered => _isPowered;
        
        private ReactiveProperty<bool> _isUnlocked = new();
        public IReactiveProperty<bool> IsUnlocked => _isUnlocked;

        private ReactiveEvent _clickEvent = new();
        public IReactiveSubscription ClickEvent => _clickEvent;

        public MinerViewModel(string id, int level, Sprite normalIcon, Sprite poweredIcon, Sprite boxIcon)
        {
            Id = id;
            Level = level;
            NormalIcon = normalIcon;
            PoweredIcon = poweredIcon;
            BoxIcon = boxIcon;
        }

        public void PointerDown()
        {
            if (_isUnlocked.Value) return;
            _clickEvent.Trigger();
        }

        public void Unlock()
        {
            _isUnlocked.Set(true);
        }

        public void SetIsMaxLevel(bool value)
        {
            _isMaxLevel.Set(value);
        }

        public void SetSize(float value)
        {
            _size.Set(value);
        }
        
        public void SetPowered(bool value)
        {
            _isPowered.Set(value);
        }
    }
}