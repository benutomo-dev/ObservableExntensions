using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    class SilentObservableCollection<T> : IReadOnlyObservableCollection<T>
    {
        IList<T> collection;

        public SilentObservableCollection(IList<T> collection)
        {
            this.collection = collection;
        }

        public T this[int index] => collection[index];


        public int Count => collection.Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool Contains(T item) => collection.Contains(item);

        public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

        public int IndexOf(T item) => collection.IndexOf(item);

        IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
    }
}
