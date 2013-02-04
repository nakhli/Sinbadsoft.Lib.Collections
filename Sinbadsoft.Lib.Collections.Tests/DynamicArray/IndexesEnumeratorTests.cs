// <copyright file="IndexesEnumeratorTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/01</date>

namespace Sinbadsoft.Lib.Collections.Tests.DynamicArray
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="IndexesEnumerator"/> class.
    /// </summary>
    [TestFixture]
    public class IndexesEnumeratorTests
    {
        [Test, Sequential]
        public void TestRange(
            [Values(new[] { 0 },
                new[] { 10258 },
                new[] { 0, 0 },
                new[] { 100, 1560 },
                new[] { 0, 0, 0 },
                new[] { 12000, 15000, 158220 },
                new[] { 0, 0, 0, 0 },
                new[] { 30, 300, 3000, 30000 })] int[] start,
            [Values(new[] { 125 },
                new[] { 11169 },
                new[] { 150, 17 },
                new[] { 320, 1587 },
                new[] { 49, 17, 9 },
                new[] { 12010, 15015, 158270 },
                new[] { 10, 7, 12, 18 },
                new[] { 35, 317, 3002, 30018 })]int[] end)
        {
            var itdata = new IterationData(start, end);
            TestRange(itdata);
            TestRange(ToReverse(itdata));
        }

        private static void TestRange(IterationData itdata)
        {
            var ienum = new IndexesEnumerator(itdata.End, itdata.Start, itdata.Reverse);

            RecursiveArrayEnumerator.Iterate(
                null, itdata.Start, itdata.End, new IndexesEnumeratorTestAction(ienum).Iterate, itdata.Reverse);
        }

        private static IterationData ToReverse(IterationData itdata)
        {
            return new IterationData(
                IndexesHelper.Add(itdata.End, -1),
                IndexesHelper.Add(itdata.Start, -1),
                !itdata.Reverse);
        }

        private struct IterationData
        {
            public readonly int[] Start;
            public readonly int[] End;
            public readonly bool Reverse;

            public IterationData(int[] start, int[] end, bool reverse = false)
            {
                this.Start = start;
                this.End = end;
                this.Reverse = reverse;
            }

            public override string ToString()
            {
                return "R: " + this.Reverse + " S: " + ArrayHelper.SequenceToString(this.Start) +
                    " E: " + ArrayHelper.SequenceToString(this.End);
            }
        }

        private class IndexesEnumeratorTestAction
        {
            private readonly IndexesEnumerator ienum;

            public IndexesEnumeratorTestAction(IndexesEnumerator ienum)
            {
                this.ienum = ienum;
            }

            public void Iterate(Array arr, params int[] indexes)
            {
                Assert.IsTrue(this.ienum.MoveNext());
                Assert.That(indexes, Is.EqualTo(this.ienum.Current));
            }
        }
    }
}