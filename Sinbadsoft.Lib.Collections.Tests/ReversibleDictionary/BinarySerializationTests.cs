// <copyright file="BinarySerializationTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/04/07</date>

namespace Sinbadsoft.Lib.Collections.Tests.ReversibleDictionary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using NUnit.Framework;

    /// <summary> Test class for <see cref="ReversibleDictionary{TKey,TValue}"/> 
    /// and <see cref="SortedReversibleDictionary{TKey,TValue}"/>
    /// binary serialization. </summary>
    [TestFixture]
    public class BinarySerializationTests
    {
        private IEnumerable<KeyValuePair<int, string>> data;

        [SetUp]
        public void SetUp()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < 10; ++i)
            {
                list.Add(new KeyValuePair<int, string>(i, "S" + i));
            }

            this.data = list;
        }

        [Test]
        public void SerializeReversibleDictionary()
        {
            var dict =
                new ReversibleDictionary<int, string>(this.data, new IntEqComparer(17), null);

            var result =
                InMemorySerializeDeserialize<int, string, ReversibleDictionary<int, string>>(dict);

            Assert.AreEqual(dict.KeyComparer, result.KeyComparer);
            Assert.AreEqual(dict.ValueComparer, result.ValueComparer);
        }

        [Test]
        public void SerializeSortedReversibleDictionary()
        {
            var dict =
                new SortedReversibleDictionary<int, string>(this.data, new IntEqComparer(17), null);

            var result =
                InMemorySerializeDeserialize<int, string, SortedReversibleDictionary<int, string>>(dict);

            Assert.AreEqual(dict.KeyComparer, result.KeyComparer);
            Assert.AreEqual(dict.ValueComparer, result.ValueComparer);
        }

        /// <summary>
        /// Asserts The given <see cref="IReversibleDictionary{TKey,TValue}"/> has the
        /// same elements and thes same elements order with its serialization/deserialization result.
        /// </summary>
        /// <typeparam name="TKey"> Dictionary key type. </typeparam>
        /// <typeparam name="TValue"> Dictionary value type. </typeparam>
        /// <typeparam name="TRdict"> Reversible dicationary type. </typeparam>
        /// <param name="rdict">
        /// The <see cref="IReversibleDictionary{TKey,TValue}"/> instance to test. 
        /// </param>
        /// <returns>
        /// The result of the serialization/deserialization operation. 
        /// </returns>
        private static TRdict InMemorySerializeDeserialize<TKey, TValue, TRdict>(TRdict rdict) 
            where TRdict : IReversibleDictionary<TKey, TValue>
        {
            using (var fs = new MemoryStream())
            {
                var bfmt = new BinaryFormatter();
                bfmt.Serialize(fs, rdict);
                fs.Seek(0, SeekOrigin.Begin);

                var result = (TRdict)bfmt.Deserialize(fs);

                Assert.That(result, Is.EqualTo(rdict));
                Assert.That(result.Reverse, Is.EqualTo(rdict.Reverse));

                return result;
            }
        }

        [Serializable]
        private struct IntEqComparer : IEqualityComparer<int>, IComparer<int>
        {
            public int Quotient;

            public IntEqComparer(int quotient)
            {
                this.Quotient = quotient;
            }

            public bool Equals(int x, int y)
            {
                return x % this.Quotient == y % this.Quotient;
            }

            public int GetHashCode(int obj)
            {
                return obj % this.Quotient;
            }

            public int Compare(int x, int y)
            {
                return (x % this.Quotient) - (y % this.Quotient);
            }
        }
    }
}
