using System;

namespace Utils
{
    public interface IReactiveProperty<out T> : IReactiveSubscription<T>
    {
        T Value { get; }
        IDisposable Bind(Action<T> onChange);
    }
    
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private T _value;
        public T Value => _value;

        private event Action<T> _onChange;

        public void Set(T value)
        {
            if (_value.Equals(value)) return;
            _value = value;
            _onChange?.Invoke(_value);
        }
        
        public IDisposable Bind(Action<T> onChange)
        {
            onChange?.Invoke(_value);
            return Subscribe(onChange);
        }
        
        public IDisposable Subscribe(Action<T> onChange)
        {
            _onChange += onChange;
            return new ReactiveSubscription<T>(this, onChange);
        }

        public void Unsubscribe(Action<T> onChange)
        {
            _onChange -= onChange;
        }
    }
}