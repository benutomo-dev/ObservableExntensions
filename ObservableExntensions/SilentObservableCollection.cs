using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    class SilentObservableCollection<T> : IReadOnlyObservableCollection<T>, IList
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


        #region ICollection
        bool ICollection.IsSynchronized => ((ICollection)collection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)collection).SyncRoot;

        void ICollection.CopyTo(Array array, int index) => ((ICollection)collection).CopyTo(array, index);
        #endregion

        #region IList
        bool IList.IsFixedSize => ((IList)collection).IsFixedSize;

        bool IList.IsReadOnly => true;

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value) => ((IList)collection).Contains(value);

        int IList.IndexOf(object value) => ((IList)collection).IndexOf(value);

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();
        #endregion
    }
}
