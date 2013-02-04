// <copyright file="IndexerTests.cs" company="Sinbadsoft">
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
    using System.Collections.Generic;
    using System.Globalization;

    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="DynamicArray{T}.Item"/> getter and setter.
    /// </summary>
    [TestFixture]
    public class IndexerTest
    {
        [Test]
        public void IndexerGetAndSet(
            [Values(new[] { 1025 },
                new[] { 33, 39 },
                new[] { 17, 28, 16 },
                new[] { 17, 9, 18, 11 },
                new[] { 9, 7, 6, 5, 17 },
                new[] { 3, 8, 7, 5, 11, 3 })]int[] counts)
        {
            var arr = ArrayHelper.New().NewArray<object>(counts).FillWith("A").As<Array>();

            // DynamicArray elements are set using indexer set
            DynamicArray<object> dynArray = ArrayHelper.ToDynamic<object>(arr);

            // DynamicArray elements are get usting indexer get
            DynamicArrayAssert.AreElementsEqual(dynArray, arr);
        }
    }
}