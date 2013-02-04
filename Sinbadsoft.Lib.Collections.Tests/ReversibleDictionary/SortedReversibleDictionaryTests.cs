// <copyright file="SortedReversibleDictionaryTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/05/04</date>

namespace Sinbadsoft.Lib.Collections.Tests.ReversibleDictionary
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class SortedReversibleDictionaryTests
    {
        [Test]
        public static void BasicTypesWithDefaultOrder()
        {
            // NOTE Keys and values should be unsorted
            // NOTE orders for keys and values should be different
            var rdict = new SortedReversibleDictionary<int, string>();
            rdict.Add(5, "S4");
            rdict.Add(1, "S5");
            rdict.Add(3, "S2");
            rdict.Add(4, "S3");
            rdict.Add(2, "S1");

            int i = 1;
            foreach (KeyValuePair<int, string> pair in rdict)
            {
                Assert.AreEqual(i++, pair.Key);
            }

            i = 1;
            foreach (KeyValuePair<string, int> rpair in rdict.Reverse)
            {
                Assert.AreEqual("S" + i++, rpair.Key);
            }
        }

        [Test]
        public static void BasicTypesWithCustomOrder()
        {
            // NOTE Keys and values should be unsorted
            // NOTE orders for keys and values should be different
            SortedReversibleDictionary<int, DateTime> rdict =
                new SortedReversibleDictionary<int, DateTime>(new ReverseComparer<int>(), new ReverseComparer<DateTime>());
            DateTime now = DateTime.Now;
            rdict.Add(5, now.AddDays(4));
            rdict.Add(1, now.AddDays(5));
            rdict.Add(3, now.AddDays(2));
            rdict.Add(4, now.AddDays(3));
            rdict.Add(2, now.AddDays(1));

            int i = 5;
            foreach (KeyValuePair<int, DateTime> pair in rdict)
            {
                Assert.AreEqual(i--, pair.Key);
            }

            i = 5;
            foreach (KeyValuePair<DateTime, int> rpair in rdict.Reverse)
            {
                Assert.AreEqual(now.AddDays(i--), rpair.Key);
            }
        }

        [Test]
        public static void CustomTypesWithCustomOrder()
        {
            // NOTE Keys and values should be unsorted
            // NOTE orders for keys and values should be different
            SortedReversibleDictionary<Person, Person> rdict =
                new SortedReversibleDictionary<Person, Person>(
                    new PersonComparer(PersonComparer.ComparerType.Height),
                    new PersonComparer(PersonComparer.ComparerType.Weight));

            rdict.Add(new Person(85, 184), new Person(85, 184));
            rdict.Add(new Person(81, 185), new Person(81, 185));
            rdict.Add(new Person(83, 182), new Person(83, 182));
            rdict.Add(new Person(84, 183), new Person(84, 183));
            rdict.Add(new Person(82, 181), new Person(82, 181));

            int i = 1;
            foreach (KeyValuePair<Person, Person> pair in rdict)
            {
                Assert.AreEqual(i++, pair.Key.Height - 180);
            }

            i = 1;
            foreach (KeyValuePair<Person, Person> rpair in rdict.Reverse)
            {
                Assert.AreEqual(i++, rpair.Key.Weight - 80);
            }
        }

        [Test]
        public static void ExceptionOnNullInitData()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SortedReversibleDictionary<int, int>(null, Comparer<int>.Default, Comparer<int>.Default));
        }

        [Test]
        public static void NoExceptionOnNullComparer()
        {
            Assert.DoesNotThrow(
                () =>
                    {
                        new SortedReversibleDictionary<int, int>(null, Comparer<int>.Default);
                        new SortedReversibleDictionary<int, int>(Comparer<int>.Default, null);
                        new SortedReversibleDictionary<int, int>(null, null);
                    });
        }

        private class ReverseComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return -Comparer<T>.Default.Compare(x, y);
            }
        }

        private class Person
        {
            public Person(int weight, int height)
            {
                this.Weight = weight;
                this.Height = height;
            }
            
            public int Weight { get; private set; }

            public int Height { get; private set; }
        }

        private class PersonComparer : IComparer<Person>
        {
            private readonly ComparerType comparerType;

            public PersonComparer(ComparerType t)
            {
                this.comparerType = t;
            }
            
            public enum ComparerType 
            { 
                Height, Weight 
            }

            public int Compare(Person x, Person y)
            {
                if (this.comparerType == ComparerType.Height)
                {
                    return x.Height - y.Height;
                }

                return x.Weight - y.Weight;
            }
        }
    }

    [TestFixture]
    public class SortedReversibleDictionaryInt2IntTests
        : CommonTests<int, int, SortedReversibleDictionary<int, int>>
    {
        public SortedReversibleDictionaryInt2IntTests()
        {
            this.DictData = TestDataHelper.Int2Int(0, 100);
            this.TestData = TestDataHelper.Int2Int(100, 200);
        }
    }

    [TestFixture]
    public class SortedReversibleDictionaryString2StringTests
        : CommonTests<string, string, SortedReversibleDictionary<string, string>>
    {
        public SortedReversibleDictionaryString2StringTests()
        {
            this.DictData = TestDataHelper.String2String(0, 100);
            this.TestData = TestDataHelper.String2String(100, 200);
        }
    }

    [TestFixture]
    public class SortedReversibleDictionaryInt2StringTests
        : CommonTests<int, string, SortedReversibleDictionary<int, string>>
    {
        public SortedReversibleDictionaryInt2StringTests()
        {
            this.DictData = TestDataHelper.Int2String(0, 100);
            this.TestData = TestDataHelper.Int2String(100, 200);
        }
    }
}