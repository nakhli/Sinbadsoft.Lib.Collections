// <copyright file="ReversibleDictionaryTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/07</date>

namespace Sinbadsoft.Lib.Collections.Tests.ReversibleDictionary
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public static class ReversibleDictionaryTests
    {
        [Test]
        public static void ExceptionOnNullInitData()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                new ReversibleDictionary<int, int>(null, EqualityComparer<int>.Default, EqualityComparer<int>.Default));
        }

        [Test]
        public static void NoExceptionOnNullComparer()
        {
            Assert.DoesNotThrow(
                () =>
                    {
                        new ReversibleDictionary<int, int>(null, EqualityComparer<int>.Default);
                        new ReversibleDictionary<int, int>(EqualityComparer<int>.Default, null);
                        new ReversibleDictionary<int, int>(null, null);
                    });
        }
    }

    [TestFixture]
    public class ReversibleDictionaryInt2IntTests
        : CommonTests<int, int, ReversibleDictionary<int, int>>
    {
        public ReversibleDictionaryInt2IntTests()
        {
            this.DictData = TestDataHelper.Int2Int(0, 100);
            this.TestData = TestDataHelper.Int2Int(100, 200);
        }
    }

    [TestFixture]
    public class ReversibleDictionaryString2StringTests
        : CommonTests<string, string, ReversibleDictionary<string, string>>
    {
        public ReversibleDictionaryString2StringTests()
        {
            this.DictData = TestDataHelper.String2String(0, 100);
            this.TestData = TestDataHelper.String2String(100, 200);
        }
    }

    [TestFixture]
    public class ReversibleDictionaryInt2StringTests
        : CommonTests<int, string, ReversibleDictionary<int, string>>
    {
        public ReversibleDictionaryInt2StringTests()
        {
            this.DictData = TestDataHelper.Int2String(0, 100);
            this.TestData = TestDataHelper.Int2String(100, 200);
        }
    }
}