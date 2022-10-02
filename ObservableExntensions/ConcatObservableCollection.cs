using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Collections.Observable
{
    class ConcatObservableCollection<T> : IReadOnlyObservableCollection<T>, IList
    {
        IReadOnlyList<T>[] collections;

        public ConcatObservableCollection(IReadOnlyObservableCollection<T>[] collections)
        {
            if (collections is null)
            {
                throw new ArgumentNullException(nameof(collections));
            }

            this.collections = collections.Cast<IReadOnlyList<T>>().ToArray();

            for (int i = 0; i < collections.Length; i++)
            {
                var collection = (INotifyCollectionChanged)collections[i];

                int takeCount = i;

                collection.CollectionChanged += (src, args) =>
                {
                    var offset = collections.Take(takeCount).Sum(c => c.Count);

                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, offset + args.NewStartingIndex));
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(args.Action, args.OldItems, offset + args.OldStartingIndex));
                            break;
                        case NotifyCollectionChangedAction.Move:
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(args.Action, args.OldItems, offset + args.NewStartingIndex, offset + args.OldStartingIndex));
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, args.OldItems, offset + args.OldStartingIndex));
                            break;
                        default:
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                            break;
                    }
                };
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                foreach (var collection in collections)
                {
                    if (index < collection.Count)
                    {
                        return collection[index];
                    }

                    index -= collection.Count;
                }

                throw new IndexOutOfRangeException();
            }
        }


        public int Count => collections.Sum(collection => collection.Count);

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool Contains(T item) => collections.Any(collection => collection.Contains(item));

        public IEnumerator<T> GetEnumerator() => collections.SelectMany(v => v).GetEnumerator();

        public int IndexOf(T item)
        {
            var indexOffset = 0;

            foreach (var collection in collections)
            {
                var currentIndex = 0;

                foreach (var current in collection)
                {
                    if (EqualityComparer<T>.Default.Equals(item, current))
                    {
                        return indexOffset + currentIndex;
                    }

                    currentIndex++;
                }

                indexOffset += collection.Count;
            }

            return -1;
        }

        IEnumerator IEnumerable.GetEnumerator() => collections.SelectMany(v => v).GetEnumerator();



        #region ICollection
        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot { get; } = new object();

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (var collection in collections)
            {
                foreach (var item in collection)
                {
                    array.SetValue(item, index++);
                }
            }
        }
        #endregion

        #region IList
        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => true;

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value)
        {
            if (typeof(T).IsValueType)
            {
                if (value is null)
                {
                    return false;
                }
                else
                {
                    return Contains((T)value);
                }
            }
            else
            {
                if (value is null || value is T)
                {
                    return Contains((T)value);
                }
                else
                {
                    return false;
                }
            }
        }

        int IList.IndexOf(object value)
        {
            if (typeof(T).IsValueType)
            {
                if (value is null)
                {
                    return -1;
                }
                else
                {
                    return IndexOf((T)value);
                }
            }
            else
            {
                if (value is null || value is T)
                {
                    return IndexOf((T)value);
                }
                else
                {
                    return -1;
                }
            }
        }

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();
        #endregion
    }
}
