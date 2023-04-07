using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.MVVM
{
    public abstract class View<T> : MonoBehaviour, IDisposableCarrier
        where T : ViewModel
    {
        private DisposableCarrier _disposables = new();

        protected T _vm;
        public T ViewModel => _vm;

        public void Bind(T vm)
        {
            _vm = vm;
            BindInner(vm);
        }
        
        protected abstract void BindInner(T vm);
        
        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        private void OnDestroy()
        {
            Dispose();
            OnDestroyInner();
        }

        protected virtual void OnDestroyInner()
        {
        }
    }

    public abstract class ViewModel : IDisposableCarrier
    {
        private DisposableCarrier _disposables = new ();
        
        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
    }

    public interface IDisposableCarrier : IDisposable
    {
        void Add(IDisposable disposable);
    }
    
    public class DisposableCarrier : List<IDisposable>, IDisposableCarrier
    {
        public void Dispose()
        {
            foreach (var disposable in this)
            {
                disposable.Dispose();
            }
            Clear();
        }
    }
    
    public static class MVVMExtensions
    {
        public static void AddTo(this IDisposable disposable, IDisposableCarrier carrier)
        {
            carrier.Add(disposable);
        }
    }
}