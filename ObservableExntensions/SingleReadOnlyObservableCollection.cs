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

    class SingleReadOnlyObservableCollection<T> : IReadOnlyObservableCollection<T>, IList
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

        #region ICollection
        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot { get; } = new object();

        void ICollection.CopyTo(Array array, int index) => array.SetValue(value, index);
        #endregion

        #region IList
        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value) => value is null ? this.value is null : value is T tValue && Contains(tValue);

        int IList.IndexOf(object value) => ((IList)this).Contains(value) ? 0 : -1;

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();
        #endregion
    }
}
