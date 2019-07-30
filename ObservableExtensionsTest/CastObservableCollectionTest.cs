using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Observable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObservableExtensionsTest
{
    [TestClass]
    public class CastObservableCollectionTest
    {
        class A
        {
            public string aValue;
        }

        class B : A
        {
            public string bValue;

            public override bool Equals(object obj)
            {
                return obj is B b && b.aValue == aValue && b.bValue == bValue;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var blist = new System.Collections.ObjectModel.ObservableCollection<B>();

            var alist = blist.UseObservableKeepingLinq().CastAsObservableCollection<A>();

            var callCount = 0;

            alist.CollectionChanged += (src, args) =>
            {
                callCount++;

                Assert.IsTrue(ReferenceEquals(src, alist));

                switch (callCount)
                {
                    case 1:
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
                        Assert.AreEqual(0, args.NewStartingIndex);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("obj_0", ((B)args.NewItems[0]).bValue);
                        break;
                    case 2:
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
                        Assert.AreEqual(1, args.NewStartingIndex);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("obj_1", ((B)args.NewItems[0]).bValue);
                        break;
                    case 3:
                        Assert.AreEqual(NotifyCollectionChangedAction.Remove, args.Action);
                        Assert.AreEqual(0, args.OldStartingIndex);
                        Assert.AreEqual(1, args.OldItems.Count);
                        Assert.AreEqual("obj_0", ((B)args.OldItems[0]).bValue);
                        break;
                    case 4:
                        Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
                        Assert.AreEqual(0, args.NewStartingIndex);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("obj_0", ((B)args.NewItems[0]).bValue);
                        break;
                    case 5:
                        Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action);
                        Assert.AreEqual(1, args.OldStartingIndex);
                        Assert.AreEqual(1, args.OldItems.Count);
                        Assert.AreEqual(1, args.NewItems.Count);
                        Assert.AreEqual("obj_1", ((B)args.OldItems[0]).bValue);
                        Assert.AreEqual("obj_2", ((B)args.NewItems[0]).bValue);
                        break;
                }

            };

            blist.Add(new B { bValue = "obj_0" });
            CollectionAssert.AreEqual(new[] { new B { bValue = "obj_0" } }, blist);

            blist.Add(new B { bValue = "obj_1" });
            CollectionAssert.AreEqual(new[] { new B { bValue = "obj_0" }, new B { bValue = "obj_1" } }, blist);

            blist.RemoveAt(0);
            CollectionAssert.AreEqual(new[] { new B { bValue = "obj_1" } }, blist);

            blist.Insert(0, new B { bValue = "obj_0" });
            CollectionAssert.AreEqual(new[] { new B { bValue = "obj_0" }, new B { bValue = "obj_1" } }, blist);

            blist[1] = new B { bValue = "obj_2" };
            CollectionAssert.AreEqual(new[] { new B { bValue = "obj_0" }, new B { bValue = "obj_2" } }, blist);
        }
    }
}
