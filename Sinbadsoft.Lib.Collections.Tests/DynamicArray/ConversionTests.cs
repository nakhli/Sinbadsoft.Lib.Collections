// <copyright file="ConversionTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/05/19</date>

namespace Sinbadsoft.Lib.Collections.Tests.DynamicArray
{
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    public class ConversionTests
    {
        private static readonly int[] Data = new[] { 5, 4, 3, 2, 1, 0 };

        [Test]
        public static void ToArray()
        {
            DynamicArray<int> darray = new DynamicArray<int>(1);
            darray.Insert(Data, 0, 0);
            Assert.That(darray, Is.EqualTo((int[])darray.ToArray()));
            Assert.That(darray, Is.EqualTo((int[])darray));
        }

        [Test]
        public static void GenericCopyTo()
        {
            DynamicArray<int> darray = new DynamicArray<int>(1);
            DynamicArray<int> dcopy = new DynamicArray<int>(1);
            darray.Insert(Data, 0, 0);
            darray.CopyTo(dcopy, 0);
            Assert.That(darray, Is.EqualTo(dcopy));
        }

        [Test]
        public static void NonGenericCopyTo()
        {
            DynamicArray<int> darray = new DynamicArray<int>(1);
            int[] copy = new int[Data.Length];
            darray.Insert(Data, 0, 0);
            ((ICollection)darray).CopyTo(copy, 0);
            Assert.That(Data, Is.EqualTo(copy));
        }
    }
}