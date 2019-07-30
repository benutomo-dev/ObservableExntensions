using System;
using System.Collections.Observable;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObservableExtensionsTest
{
    [TestClass]
    public class ConcatObservableCollectionTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list1 = new ObservableCollection<string>();
            var list2 = new ObservableCollection<string>();
            var list3 = new ObservableCollection<string>();


            var concatList = new[] { list1, list2, list3 }.Concat();

            var callCount = 0;

            concatList.CollectionChanged += (src, args) =>
            {
                callCount++;

                Assert.IsTrue(ReferenceEquals(src, concatList));

                switch (callCount)
                {
                    case 1:
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
                        Assert.AreEqual(0, args.NewStartingIndex);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("apple", args.NewItems[0]);
                        break;
                    case 2:
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
                        Assert.AreEqual(0, args.NewStartingIndex);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("banana", args.NewItems[0]);
                        break;
                    case 3:
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
                        Assert.AreEqual(2, args.NewStartingIndex);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("orange", args.NewItems[0]);
                        break;
                    case 4:
                        Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action);
                        Assert.AreEqual(1, args.OldStartingIndex);
                        Assert.AreEqual(1, args.OldItems.Count);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("apple", args.OldItems[0]);
                        Assert.AreEqual("pineapple", args.NewItems[0]);
                        break;
                }
            };

            list3.Add("apple");

            list1.Add("banana");

            list3.Add("orange");

            list3[0] = "pineapple";
        }
    }
}
