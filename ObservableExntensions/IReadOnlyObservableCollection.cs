using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace System.Collections.Observable
{
    public interface IReadOnlyObservableCollection<T> : IReadOnlyList<T>, INotifyCollectionChanged
    {
        int IndexOf(T item);

        bool Contains(T item);
    }
}
