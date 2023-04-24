using System;
using UnityEngine.UI;

namespace Utils.Reactive
{
    public interface IReactiveSubscription
    {
        IDisposable Subscribe(Action onChange);
        void Unsubscribe(Action onChange);
    }
    
    public interface IReactiveSubscription<out T>
    {
        IDisposable Subscribe(Action<T> onChange);
        void Unsubscribe(Action<T> onChange);
    }
    
    public interface IReactiveSubscription<out T1, out T2>
    {
        IDisposable Subscribe(Action<T1, T2> onChange);
        void Unsubscribe(Action<T1, T2> onChange);
    }
    
    public class ReactiveSubscription : IDisposable
    {
        private readonly IReactiveSubscription _event;
        private readonly Action _onChange;

        public ReactiveSubscription(IReactiveSubscription @event, Action onChange)
        {
            _event = @event;
            _onChange = onChange;
        }
        
        public void Dispose()
        {
            _event.Unsubscribe(_onChange);
        }
    }
    
    public class ReactiveSubscription<T> : IDisposable
    {
        private readonly IReactiveSubscription<T> _event;
        private readonly Action<T> _onChange;

        public ReactiveSubscription(IReactiveSubscription<T> @event, Action<T> onChange)
        {
            _event = @event;
            _onChange = onChange;
        }
        
        public void Dispose()
        {
            _event.Unsubscribe(_onChange);
        }
    }
    
    public class ReactiveSubscription<T1, T2> : IDisposable
    {
        private readonly IReactiveSubscription<T1, T2> _event;
        private readonly Action<T1, T2> _onChange;

        public ReactiveSubscription(IReactiveSubscription<T1, T2> @event, Action<T1, T2> onChange)
        {
            _event = @event;
            _onChange = onChange;
        }
        
        public void Dispose()
        {
            _event.Unsubscribe(_onChange);
        }
    }

    public static class ReactiveExtensions
    {
        public static IDisposable Subscribe(this Button button, Action onClick)
        {
            var reactiveEvent = new ReactiveEvent();
            button.onClick.AddListener(reactiveEvent.Trigger);
            return reactiveEvent.Subscribe(onClick);
        }
    }
}