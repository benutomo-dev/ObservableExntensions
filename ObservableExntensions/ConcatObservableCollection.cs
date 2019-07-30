using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Collections.Observable
{
    class ConcatObservableCollection<T> : IReadOnlyObservableCollection<T>
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
    }
}
