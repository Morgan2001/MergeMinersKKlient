using System;

namespace Utils
{
    public interface IReactiveEventTrigger
    {
        void Trigger();
    }
    
    public interface IReactiveEventTrigger<in T>
    {
        void Trigger(T value);
    }
    
    public interface IReactiveEventTrigger<in T1, in T2>
    {
        void Trigger(T1 value1, T2 value2);
    }
    
    public class ReactiveEvent : IReactiveSubscription, IReactiveEventTrigger
    {
        private event Action _onTrigger;

        public void Trigger()
        {
            _onTrigger?.Invoke();
        }
        
        public IDisposable Subscribe(Action onTrigger)
        {
            _onTrigger += onTrigger;
            return new ReactiveSubscription(this, onTrigger);
        }

        public void Unsubscribe(Action onChange)
        {
            _onTrigger -= onChange;
        }
    }
    
    public class ReactiveEvent<T> : IReactiveSubscription<T>, IReactiveEventTrigger<T>
    {
        private event Action<T> _onTrigger;

        public void Trigger(T value)
        {
            _onTrigger?.Invoke(value);
        }
        
        public IDisposable Subscribe(Action<T> onTrigger)
        {
            _onTrigger += onTrigger;
            return new ReactiveSubscription<T>(this, onTrigger);
        }

        public void Unsubscribe(Action<T> onChange)
        {
            _onTrigger -= onChange;
        }
    }
    
    public class ReactiveEvent<T1, T2> : IReactiveSubscription<T1, T2>, IReactiveEventTrigger<T1, T2>
    {
        private event Action<T1, T2> _onTrigger;

        public void Trigger(T1 value1, T2 value2)
        {
            _onTrigger?.Invoke(value1, value2);
        }
        
        public IDisposable Subscribe(Action<T1, T2> onTrigger)
        {
            _onTrigger += onTrigger;
            return new ReactiveSubscription<T1, T2>(this, onTrigger);
        }

        public void Unsubscribe(Action<T1, T2> onChange)
        {
            _onTrigger -= onChange;
        }
    }
}