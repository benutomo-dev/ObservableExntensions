using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    public class CastObservableCollection<CastToType> : IReadOnlyObservableCollection<CastToType>, IList
    {
        private IList collection;

        public int Count => collection.Count;

        public CastToType this[int index] => (CastToType)collection[index];

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public CastObservableCollection(INotifyCollectionChanged collection)
        {
            this.collection = (collection ?? throw new ArgumentNullException(nameof(collection))) as IList ?? throw new ArgumentException(nameof(collection));

            collection.CollectionChanged += (src, args) => CollectionChanged?.Invoke(this, args);
        }

        public int IndexOf(CastToType item) => collection.IndexOf(item);

        public bool Contains(CastToType item) => collection.Contains(item);

        public IEnumerator<CastToType> GetEnumerator()
        {
            foreach (CastToType item in collection)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }


        #region ICollection
        bool ICollection.IsSynchronized => ((ICollection)collection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)collection).SyncRoot;

        void ICollection.CopyTo(Array array, int index) => ((ICollection)collection).CopyTo(array, index);
        #endregion

        #region IList
        bool IList.IsFixedSize => ((IList)collection).IsFixedSize;

        bool IList.IsReadOnly => true;

        object IList.this[int index] { get => this[index]!; set => throw new NotSupportedException(); }

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value) => ((IList)collection).Contains(value);

        int IList.IndexOf(object value) => ((IList)collection).IndexOf(value);

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();
        #endregion
    }

    class CastObservableCollection<CastToType, CastFromType> : IReadOnlyObservableCollection<CastToType>, IList where CastFromType : CastToType
    {
        private IList<CastFromType> collection;

        public int Count => collection.Count;

        public CastToType this[int index] => (CastToType)collection[index];

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public CastObservableCollection(INotifyCollectionChanged collection)
        {
            this.collection = (collection ?? throw new ArgumentNullException(nameof(collection))) as IList<CastFromType> ?? throw new ArgumentException(nameof(collection));

            collection.CollectionChanged += (src, args) => CollectionChanged?.Invoke(this, args);
        }

        public int IndexOf(CastToType item) => (item is CastFromType fromTypeItem) ? collection.IndexOf(fromTypeItem) : -1;

        public bool Contains(CastToType item) => item is CastFromType fromTypeItem && collection.Contains(fromTypeItem);

        public IEnumerator<CastToType> GetEnumerator()
        {
            foreach (CastToType item in collection)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }



        #region ICollection
        bool ICollection.IsSynchronized => ((ICollection)collection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)collection).SyncRoot;

        void ICollection.CopyTo(Array array, int index) => ((ICollection)collection).CopyTo(array, index);
        #endregion

        #region IList
        bool IList.IsFixedSize => ((IList)collection).IsFixedSize;

        bool IList.IsReadOnly => true;

        object IList.this[int index] { get => this[index]!; set => throw new NotSupportedException(); }

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
