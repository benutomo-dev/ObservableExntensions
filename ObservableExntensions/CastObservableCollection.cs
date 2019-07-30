using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    public class CastObservableCollection<CastToType> : IReadOnlyObservableCollection<CastToType>
    {
        private IList collection;

        public int Count => collection.Count;

        public CastToType this[int index] => (CastToType)collection[index];

        public event NotifyCollectionChangedEventHandler CollectionChanged;

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
    }

    class CastObservableCollection<CastToType, CastFromType> : IReadOnlyObservableCollection<CastToType> where CastFromType : CastToType
    {
        private IList<CastFromType> collection;

        public int Count => collection.Count;

        public CastToType this[int index] => (CastToType)collection[index];

        public event NotifyCollectionChangedEventHandler CollectionChanged;

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
    }
}
