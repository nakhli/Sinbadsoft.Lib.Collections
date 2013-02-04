// <copyright file="CommonTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/04/17</date>

namespace Sinbadsoft.Lib.Collections.Tests.ReversibleDictionary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    using NUnit.Framework;

    /// <summary>
    /// Fundamental tests for <see cref="IReversibleDictionary{TKey,TValue}"/> implementations.
    /// </summary>
    /// <typeparam name="TKey"> The type of the dictionary's key elements. </typeparam>
    /// <typeparam name="TValue"> The type of the dictionary's value elements. </typeparam>
    /// <typeparam name="TRdict"> The reversible dictionary type to test. </typeparam>
    /// <remarks>
    /// This class adresses the testing of the contract defined by <see cref="IReversibleDictionary{TKey,TValue}"/>.
    /// A specific implementation of <see cref="IReversibleDictionary{TKey,TValue}"/> may need other tests for
    /// its specific implementation features (such as order, complexity, etc.).
    /// </remarks>
    public abstract class CommonTests<TKey, TValue, TRdict> 
        where TRdict : IReversibleDictionary<TKey, TValue>, new()
    {
        private readonly Func<TRdict> create;
        private readonly Func<TRdict> createAndInit;

        protected CommonTests()
        {
            this.create = () => new TRdict();
            this.createAndInit = () =>
                {
                    var rdict = this.create();
                    foreach (var pair in this.DictData)
                    {
                        rdict.Add(pair);    
                    }

                    return rdict;
                };
        }

        /// <summary>
        /// Gets or sets data used to intialize the reversible dictionary to be tested. Should not have
        /// elements in common with <see cref="TestData"/>. Should have at least 2 elements.
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> DictData { get; protected set; }


        /// <summary>
        /// Gets or sets data used to test operations on the reversible dictionary. Should not have
        /// elements in common with <see cref="DictData"/>. Should have at least 2 elements.
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> TestData { get; protected set; }

        [Test]
        public void InitFromEnumerable()
        {
            var rdict = this.createAndInit();
            Assert.That(rdict, Is.EquivalentTo(this.DictData));
        }

        [Test]
        public void AddRemoveCount()
        {
            var rdict = this.createAndInit();

            foreach (KeyValuePair<TKey, TValue> pair in this.TestData)
            {
                int initialCount = rdict.Count;
                rdict.Add(pair.Key, pair.Value);
                CheckAddedGeneric(rdict, pair, initialCount);

                Assert.IsTrue(rdict.Remove(pair.Key));
                CheckRemovedGeneric(rdict, pair, initialCount);

                rdict[pair.Key] = pair.Value;
                CheckAddedGeneric(rdict, pair, initialCount);

                Assert.IsTrue(rdict.Remove(pair));
                CheckRemovedGeneric(rdict, pair, initialCount);

                rdict.Add(pair);
                CheckAddedGeneric(rdict, pair, initialCount);
            }
        }

        [Test]
        public void CopyTo()
        {
            var rdict = this.createAndInit();
            var copyHere = new KeyValuePair<TKey, TValue>[rdict.Count];
            rdict.CopyTo(copyHere, 0);
            Assert.That(copyHere, Is.EqualTo(rdict));
        }

        [Test]
        public void Clear()
        {
            IReversibleDictionary<TKey, TValue> rdict = this.createAndInit();
            Assert.IsNotEmpty(rdict);
            rdict.Clear();
            Assert.AreEqual(0, rdict.Count);
            Assert.AreEqual(0, rdict.Reverse.Count);
            Assert.IsEmpty(rdict);
            Assert.DoesNotThrow(rdict.Clear);
        }

        [Test]
        public void NonGenericClear()
        {
            var rdict = (IReversibleDictionary)this.createAndInit();
            Assert.IsNotEmpty(rdict);
            rdict.Clear();
            Assert.AreEqual(0, rdict.Count);
            Assert.AreEqual(0, rdict.Reverse.Count);
            Assert.IsEmpty(rdict);
            Assert.DoesNotThrow(rdict.Clear);
        }

        [Test]
        public void ExceptionOnGetAbsentKeyOrValue()
        {
            IReversibleDictionary<TKey, TValue> rdict = this.createAndInit();
            foreach (var pair in this.TestData.Take(2))
            {
                var reversePair = new KeyValuePair<TValue, TKey>(pair.Value, pair.Key);
                DictionaryAssert.ThrowsOnGetItem(rdict, pair.Key);
                DictionaryAssert.ThrowsOnGetItem(rdict.Reverse, reversePair.Key);
            }
        }

        [Test]
        public void ExceptionOnAddPresentKeyOrValue()
        {
            IReversibleDictionary<TKey, TValue> rdict = this.createAndInit();
            foreach (var pair in rdict.Take(2).ToList())
            {
                var reversePair = new KeyValuePair<TValue, TKey>(pair.Value, pair.Key);
                DictionaryAssert.ThrowsOnAdd(rdict, pair);
                DictionaryAssert.ThrowsOnAdd(rdict.Reverse, reversePair);
            }
        }

        [Test]
        public void Indexer()
        {
            IReversibleDictionary<TKey, TValue> rdict = this.createAndInit();
            KeyValuePair<TKey, TValue> pair = this.TestData.First();
            KeyValuePair<TKey, TValue> oldpair = this.TestData.Skip(1).First();

            TKey key1 = pair.Key, key2 = oldpair.Key;
            TValue val1 = pair.Value, val2 = oldpair.Value;

            // key1, key2 and val1, val2 are not in the dictionary
            rdict[key1] = val1;
            EnsureNewValuesInAndOldValuesOut(rdict, key1, val1, key2, val2);

            // (key1, val1) pair is in the dictionary
            rdict[key1] = val2;
            EnsureNewValuesInAndOldValuesOut(rdict, key1, val2, key2, val1);

            // (key1, val2) pair is in the dictionary
            rdict.Reverse[val2] = key2;
            EnsureNewValuesInAndOldValuesOut(rdict, key2, val2, key1, val1);
        }

        [Test]
        public void NonGenericExceptionOnKeyOrValueOfWrongType()
        {
            var rdict = (IReversibleDictionary)this.createAndInit();

            // System.Collections.IDictionary.Add explicit impl
            Assert.Throws<ArgumentException>(() => rdict.Add(new CustomPrivateType(), default(TValue)));
            Assert.Throws<ArgumentException>(() => rdict.Add(default(TKey), new CustomPrivateType()));

            // System.Collections.IDictionary.Item explicit impl
            Assert.Throws<ArgumentException>(() => rdict[new CustomPrivateType()] = default(TValue));
            Assert.Throws<ArgumentException>(() => { object val = rdict[new CustomPrivateType()]; });
            Assert.Throws<ArgumentException>(() => rdict[default(TKey)] = new CustomPrivateType());
        }

        [Test]
        public void NonGenericNoExceptionOnKeyOrValueOfWrongType()
        {
            IReversibleDictionary rdict = (IReversibleDictionary)this.createAndInit();
            Assert.DoesNotThrow(() => rdict.Remove(new CustomPrivateType()));
            Assert.DoesNotThrow(() => { bool result = rdict.Contains(new CustomPrivateType()); });
        }

        [Test]
        public void NonGenericCopyTo()
        {
            IReversibleDictionary<TKey, TValue> rdict = this.createAndInit();
            Array copyHereUntyped = new KeyValuePair<TKey, TValue>[rdict.Count];
            ((IReversibleDictionary)rdict).CopyTo(copyHereUntyped, 0);
            Assert.That(copyHereUntyped, Is.EqualTo(rdict));
        }

        [Test]
        public void NonGenericAddRemoveCount()
        {
            var rdict = (IReversibleDictionary)this.createAndInit();

            foreach (KeyValuePair<TKey, TValue> pair in this.TestData)
            {
                var entry = new DictionaryEntry(pair.Key, pair.Value);
                int initialCount = rdict.Count;
                rdict.Add(pair.Key, pair.Value);
                CheckAddedNonGeneric(rdict, entry, initialCount);

                rdict.Remove(entry.Key);
                CheckRemovedNonGeneric(rdict, entry, initialCount);

                rdict[pair.Key] = pair.Value;
                CheckAddedNonGeneric(rdict, entry, initialCount);
            }
        }

        [Test]
        public void ExceptionOnNullKeyOrValue()
        {
            var rdict = this.create();
            var pair = this.TestData.First();

            if (typeof(TKey).IsClass)
            {
                Assert.Throws<ArgumentNullException>(() => rdict[default(TKey)] = pair.Value);
                Assert.Throws<ArgumentNullException>(() => { TValue val = rdict[default(TKey)]; });
                Assert.Throws<ArgumentNullException>(() => rdict.Add(default(TKey), pair.Value));
            }

            if (typeof(TValue).IsClass)
            {
                Assert.Throws<ArgumentNullException>(() => rdict[pair.Key] = default(TValue));
                Assert.Throws<ArgumentNullException>(() => rdict.Add(pair.Key, default(TValue)));
            }
        }

        private static void CheckAddedGeneric(
            IReversibleDictionary<TKey, TValue> rdict,
            KeyValuePair<TKey, TValue> pair,
            int initialCount)
        {
            DictionaryAssert.Contains(rdict, pair);
            DictionaryAssert.Contains(rdict.Reverse, new KeyValuePair<TValue, TKey>(pair.Value, pair.Key));
            Assert.AreEqual(rdict.Reverse.Count, rdict.Count);
            Assert.AreEqual(initialCount + 1, rdict.Count);
        }

        private static void CheckRemovedGeneric(
            IReversibleDictionary<TKey, TValue> rdict,
            KeyValuePair<TKey, TValue> pair,
            int initialCount)
        {
            DictionaryAssert.DoesNotContainKey(rdict, pair.Key);
            DictionaryAssert.DoesNotContainKey(rdict.Reverse, pair.Value);
            Assert.AreEqual(rdict.Reverse.Count, rdict.Count);
            Assert.AreEqual(initialCount, rdict.Count);
            Assert.IsFalse(rdict.Remove(pair));
        }

        private static void CheckAddedNonGeneric(
            IReversibleDictionary rdict,
            DictionaryEntry entry,
            int initialCount)
        {
            Assert.IsTrue(rdict.Contains(entry.Key));
            Assert.IsTrue(rdict.Reverse.Contains(entry.Value));
            Assert.AreEqual(rdict.Reverse.Count, rdict.Count);
            Assert.AreEqual(initialCount + 1, rdict.Count);
        }

        private static void CheckRemovedNonGeneric(
            IReversibleDictionary rdict,
            DictionaryEntry entry,
            int initialCount)
        {
            Assert.IsFalse(rdict.Contains(entry.Key));
            Assert.IsFalse(rdict.Reverse.Contains(entry.Value));
            Assert.AreEqual(rdict.Reverse.Count, rdict.Count);
            Assert.AreEqual(initialCount, rdict.Count);
        }

        private static void EnsureNewValuesInAndOldValuesOut(IReversibleDictionary<TKey, TValue> dict, TKey key, TValue val, TKey oldKey, TValue oldVal)
        {
            DictionaryAssert.Contains(dict, new KeyValuePair<TKey, TValue>(key, val));
            DictionaryAssert.Contains(dict.Reverse, new KeyValuePair<TValue, TKey>(val, key));

            DictionaryAssert.DoesNotContainKey(dict, oldKey);
            DictionaryAssert.DoesNotContainKey(dict.Reverse, oldVal);

            DictionaryAssert.DoesNotContain(dict, new KeyValuePair<TKey, TValue>(oldKey, val));
            DictionaryAssert.DoesNotContain(dict.Reverse, new KeyValuePair<TValue, TKey>(val, oldKey));
        }

        private class CustomPrivateType
        {
        }
    }
}
