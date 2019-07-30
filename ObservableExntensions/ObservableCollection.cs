using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    public class ObservableCollection<T> : IObservableCollection<T>
    {
        System.Collections.ObjectModel.ObservableCollection<T> collection;

        public ObservableCollection() : this(new System.Collections.ObjectModel.ObservableCollection<T>())
        {
        }

        internal ObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));

            collection.CollectionChanged += (src, args) => CollectionChanged?.Invoke(this, args);
        }

        public T this[int index]
        {
            get => collection[index];
            set => collection[index] = value;
        }

        public int Count => collection.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item) => collection.Add(item);

        public void Clear() => collection.Clear();

        public bool Contains(T item) => collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => collection.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

        public int IndexOf(T item) => collection.IndexOf(item);

        public void Insert(int index, T item) => collection.Insert(index, item);

        public void Move(int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item) => collection.Remove(item);

        public void RemoveAt(int index) => collection.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();
    }
}
