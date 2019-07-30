using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Collections.Observable
{
    public static class ObservableCollectionExtensions
    {
        public static IReadOnlyObservableCollection<TResult> Cast<TResult>(this INotifyCollectionChanged collection)
        {
            if (collection is IReadOnlyObservableCollection<TResult> readOnlyObservableCollection)
            {
                return readOnlyObservableCollection;
            }

            foreach (var interfaceType in collection.GetType().GetInterfaces())
            {
                if (interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IList<>)
                    && typeof(TResult).IsAssignableFrom(interfaceType.GetGenericArguments()[0]))
                {
                    return (IReadOnlyObservableCollection<TResult>)Activator.CreateInstance(typeof(CastObservableCollection<,>).MakeGenericType(typeof(TResult), interfaceType.GetGenericArguments()[0]), collection);
                }
            }

            return new CastObservableCollection<TResult>(collection);
        }

        public static IReadOnlyObservableCollection<TResult> Select<TSource, TResult>(this IReadOnlyObservableCollection<TSource> collection, Func<TSource, TResult> selector)
        {
            return new SelectObservableCollection<TSource, TResult>(collection, selector);
        }

        public static IReadOnlyObservableCollection<TResult> Concat<TResult>(this IReadOnlyObservableCollection<TResult>[] collections)
        {
            return new ConcatObservableCollection<TResult>(collections);
        }

        public static IReadOnlyObservableCollection<TResult> UseObservableKeepingLinq<TResult>(this System.Collections.ObjectModel.ObservableCollection<TResult> collection)
        {
            return new ObservableCollection<TResult>(collection);
        }

        public static IReadOnlyObservableCollection<T> ToFreezedObservableCollection<T>(this IEnumerable<T> collection)
        {
            if (collection is ICollection<T> strictCollection)
            {
                if (strictCollection.IsReadOnly && strictCollection is IList<T> list)
                {
                    return new SilentObservableCollection<T>(list);
                }
                else
                {
                    var array = new T[strictCollection.Count];
                    strictCollection.CopyTo(array, 0);
                    return new SilentObservableCollection<T>(array);
                }
            }
            else
            {
                return new SilentObservableCollection<T>(collection.ToArray());
            }
        }
    }
}
