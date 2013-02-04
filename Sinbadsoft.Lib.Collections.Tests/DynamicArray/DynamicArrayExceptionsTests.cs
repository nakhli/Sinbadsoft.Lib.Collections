// <copyright file="DynamicArrayExceptionsTests.cs" company="Sinbadsoft">
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
    using System.Collections;

    using NUnit.Framework;

    [TestFixture]
    public static class DynamicArrayExceptionsTests
    {
        [Test]
        public static void ExceptionOnCopyToOfMultiDim()
        {
            Assert.Throws<RankException>(
                () =>
                    {
                        var dynArray = new DynamicArray<object>(2);
                        var copyActual = new object[dynArray.Count];
                        ((ICollection)dynArray).CopyTo(copyActual, 0);
                    });
        }
        
        [Test]
        public static void ExceptionOnTooLargeRank()
        {
            Assert.Throws<TypeLoadException>(() => new DynamicArray<object>(121));
        }

        [Test]
        public static void ExceptionOnIncorrectIndexesSize()
        {
            var dynArray = new DynamicArray<int>(3);
            var tooLargeIndexes = new int[dynArray.Rank + 1];
            var tooSmallIndexes = new int[dynArray.Rank - 1];

            // Indexer get
            Assert.Throws<ArgumentException>(() => { int i = dynArray[tooSmallIndexes]; });
            Assert.Throws<ArgumentException>(() => { int i = dynArray[tooLargeIndexes]; });

            // Indexer set
            Assert.Throws<ArgumentException>(() => dynArray[tooLargeIndexes] = 10);
            Assert.Throws<ArgumentException>(() => dynArray[tooSmallIndexes] = 10);

            // GetValue
            Assert.Throws<ArgumentException>(() => { int i = dynArray.GetValue(tooSmallIndexes); });
            Assert.Throws<ArgumentException>(() => { int i = dynArray.GetValue(tooLargeIndexes); });

            // SetValue
            Assert.Throws<ArgumentException>(() => dynArray.SetValue(10, tooLargeIndexes));
            Assert.Throws<ArgumentException>(() => dynArray.SetValue(10, tooSmallIndexes));

            // Insert
            Assert.Throws<ArgumentException>(
                () => dynArray.Insert(new[,,] { { { 1 }, { 2 } }, { { 1 }, { 2 } } }, 0, tooSmallIndexes));
            Assert.Throws<ArgumentException>(
                () => dynArray.Insert(new[,,] { { { 1 }, { 2 } }, { { 1 }, { 2 } } }, 0, tooLargeIndexes));

            // Resize
            Assert.Throws<ArgumentException>(() => dynArray.Resize(tooLargeIndexes));
            Assert.Throws<ArgumentException>(() => dynArray.Resize(tooSmallIndexes));

            // Extend
            Assert.Throws<ArgumentException>(() => dynArray.Extend(tooLargeIndexes));
            Assert.Throws<ArgumentException>(() => dynArray.Extend(tooSmallIndexes));
       }

        [Test]
        public static void ExceptionOnInvalidDimension()
        {
            DynamicArray<string> dynArray = new DynamicArray<string>(2);
            
            // ExtendDim
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.ExtendDim(dynArray.Rank + 1, 4));
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.ExtendDim(-1, 4));

            // ExtendDim
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.ResizeDim(dynArray.Rank + 1, 4));
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.ResizeDim(-1, 4));

            // ExtendDim
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.GetCapacity(dynArray.Rank + 1));
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.GetCapacity(-1));

            // ExtendDim
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.GetCount(dynArray.Rank + 1));
            Assert.Throws<IndexOutOfRangeException>(() => dynArray.GetCount(-1));
        }
    }
}