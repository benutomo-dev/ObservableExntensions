using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    public static class SingleReadOnlyObservableCollection
    {
        public static IReadOnlyObservableCollection<T> Create<T>(T value) => new SingleReadOnlyObservableCollection<T>(value);
    }

    class SingleReadOnlyObservableCollection<T> : IReadOnlyObservableCollection<T>
    {
        T value;

        public SingleReadOnlyObservableCollection(T value)
        {
            this.value = value;
        }

        public T this[int index]
        {
            get
            {
                if (index != 0) throw new IndexOutOfRangeException();
                return value;
            }
        }

        public int Count => 1;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool Contains(T item) => EqualityComparer<T>.Default.Equals(value, item);

        public IEnumerator<T> GetEnumerator()
        {
            yield return value;
        }

        public int IndexOf(T item) => EqualityComparer<T>.Default.Equals(value, item) ? 0 : -1;

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return value;
        }
    }
}
