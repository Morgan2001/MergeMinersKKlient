using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils.Reactive
{
    public class ReactiveList<T> : IReadOnlyList<T>, IReactiveProperty<int>
    {
        private List<T> _list = new();
        
        public int Value => Count;

        private event Action<int> _onChange;

        public void Add(T item)
        {
            _list.Add(item);
            _onChange?.Invoke(Count);
        }
        
        public void AddRange(IEnumerable<T> items)
        {
            _list.AddRange(items);
            _onChange?.Invoke(Count);
        }
        
        public void Remove(T item)
        {
            _list.Remove(item);
            _onChange?.Invoke(Count);
        }

        public void Clear()
        {
            _list.Clear();
            _onChange?.Invoke(Count);
        }

        public IDisposable Bind(Action<int> onChange)
        {
            onChange?.Invoke(Value);
            return Subscribe(onChange);
        }
        
        public IDisposable Subscribe(Action<int> onChange)
        {
            _onChange += onChange;
            return new ReactiveSubscription<int>(this, onChange);
        }

        public void Unsubscribe(Action<int> onChange)
        {
            _onChange -= onChange;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _list.Count;

        public T this[int index] => _list[index];
    }
}