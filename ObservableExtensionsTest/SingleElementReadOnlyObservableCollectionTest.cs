using System;
using System.Collections;
using System.Collections.Observable;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObservableExtensionsTest
{
    [TestClass]
    public class SingleElementReadOnlyObservableCollectionTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var collection = SingleReadOnlyObservableCollection.Create("value");

            Assert.AreEqual(1, collection.Count);

            Assert.AreEqual("value", collection[0]);

            Assert.IsTrue(collection.Contains("value"));
            Assert.IsFalse(collection.Contains("apple"));

            Assert.AreEqual(0, collection.IndexOf("value"));
            Assert.AreEqual(-1, collection.IndexOf("apple"));

            CollectionAssert.AreEqual(new[] { "value" }, collection.ToArray());
        }
    }
}
