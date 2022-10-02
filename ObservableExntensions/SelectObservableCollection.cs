using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Collections.Observable
{
    class SelectObservableCollection<TSource, TResult> : IReadOnlyObservableCollection<TResult>, IList
    {
        private List<TResult> collection;

        public int Count => collection.Count;

        public TResult this[int index] => collection[index];

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        Func<TSource, TResult> create;

        public SelectObservableCollection(IReadOnlyObservableCollection<TSource> collection, Func<TSource, TResult> selecter)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            this.create = selecter ?? throw new ArgumentNullException(nameof(selecter));

            this.collection = new List<TResult>(collection.Count);

            Collection_CollectionChanged(collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            collection.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            List<TResult>? transNewItems = null;
            List<TResult>? transOldItems = null;
            NotifyCollectionChangedEventArgs? transArgs = null;

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    transNewItems = args.NewItems.Cast<TSource>().Select(create).ToList();
                    transArgs = new NotifyCollectionChangedEventArgs(args.Action, transNewItems, args.NewStartingIndex);
                    collection.InsertRange(args.NewStartingIndex, transNewItems);
                    CollectionChanged?.Invoke(this, transArgs);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    transOldItems = collection.Skip(args.OldStartingIndex).Take(args.OldItems.Count).ToList();
                    transArgs = new NotifyCollectionChangedEventArgs(args.Action, transOldItems, args.OldStartingIndex);
                    collection.RemoveRange(args.OldStartingIndex, args.OldItems.Count);
                    CollectionChanged?.Invoke(this, transArgs);
                    break;
                case NotifyCollectionChangedAction.Move:
                    transOldItems = collection.Skip(args.OldStartingIndex).Take(args.OldItems.Count).ToList();
                    transArgs = new NotifyCollectionChangedEventArgs(args.Action, transOldItems, args.NewStartingIndex, args.OldStartingIndex);
                    collection.RemoveRange(args.OldStartingIndex, transOldItems.Count);
                    collection.InsertRange(args.NewStartingIndex, transOldItems);
                    CollectionChanged?.Invoke(this, transArgs);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    transNewItems = args.NewItems.Cast<TSource>().Select(create).ToList();
                    transOldItems = collection.Skip(args.OldStartingIndex).Take(args.OldItems.Count).ToList();
                    transArgs = new NotifyCollectionChangedEventArgs(args.Action, transNewItems, transOldItems, args.OldStartingIndex);
                    if (args.NewItems.Count == args.OldItems.Count)
                    {
                        for (int i = 0; i < args.NewItems.Count; i++)
                        {
                            collection[args.OldStartingIndex + i] = transNewItems[i];
                        }
                    }
                    else
                    {
                        collection.RemoveRange(args.OldStartingIndex, args.OldItems.Count);
                        collection.InsertRange(args.OldStartingIndex, transNewItems);
                    }
                    CollectionChanged?.Invoke(this, transArgs);
                    break;
                default:
                    collection.Clear();
                    collection.AddRange(((IEnumerable<TSource>)sender).Select(create));
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
            }
        }

        public int IndexOf(TResult item) => collection.IndexOf(item);

        public bool Contains(TResult item) => collection.Contains(item);

        public IEnumerator<TResult> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => collection.GetEnumerator();



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
