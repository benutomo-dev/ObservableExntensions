using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Observable
{
    public interface IObservableCollection<T> : IList<T>, IReadOnlyObservableCollection<T>
    {
        void Move(int oldIndex, int newIndex);
    }
}
