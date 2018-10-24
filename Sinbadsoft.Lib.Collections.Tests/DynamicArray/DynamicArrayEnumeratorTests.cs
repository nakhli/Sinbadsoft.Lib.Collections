// <copyright file="DynamicArrayEnumeratorTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/04/14</date>

namespace Sinbadsoft.Lib.Collections.Tests.DynamicArray
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="DynamicArray{T}"/> enumeration.
    /// </summary>
    /// <remarks> This class ensure the specification stated in 
    /// <see url="http://msdn.microsoft.com/en-us/library/system.collections.ienumerator.aspx"/> is correctly 
    /// implemented.</remarks>
    [TestFixture]
    public class DynamicArrayEnumeratorTests
    {
        public static IEnumerable<Action<DynamicArray<int>>> InvalidateActions
        {
            get
            {
                // Invalidate - Set Value
                yield return da => da.SetValue(10, 10, 120);

                // "Invalidate - Indexer Set",
                yield return da => da[10, 10] = 120;

                // Invalidate - Insert
                yield return da => da.Insert(new[,] { { 10 } }, 0, 3, 3);
                
                // Invalidate - Clear
                yield return da => da.Clear();

                // Invalidate - Resize : Grow
                yield return da => da.Resize(100, 100);
                
                // Invalidate - Resize : Shrink
                yield return da => da.Resize(2, 2);
                
                // Invalidate - ResizeDim : Grow
                yield return da => da.ResizeDim(0, 100);
                
                // Invalidate - ResizeDim : Shrink
                yield return da => da.ResizeDim(1, 3);

                // Invalidate - Extend
                yield return da => da.Extend(10, 10);

                // Invalidate - ExtendDim
                yield return da => da.ExtendDim(0, 20);
            }
        }

        [Test]
        public void AsGenericCollection()
        {
            var data = new[] { 3, 2, 4, 5, 7, -3, -2, -4, -5, -7 };
            var darray = new DynamicArray<int>(1);
            darray.Insert(data, 0, 0);

            Assert.That(darray, Is.EqualTo(data));
        }

        [Test]
        public void AsNonGenericCollection()
        {
            var data = new[] { 3, 2, 4, 5, 7, -3, -2, -4, -5, -7 };
            var darray = new DynamicArray<int>(1);
            darray.Insert(data, 0, 0);

            int i = 0;
            foreach (int element in (IEnumerable)darray)
            {
                Assert.AreEqual(data[i++], element);
            }
        }

        [Test]
        public void InvalidateEnumerator(
            [ValueSource("InvalidateActions")]Action<DynamicArray<int>> invalidator)
        {
            // Build a DynamicArray
            var darray = new DynamicArray<int>(2);
            darray.Insert(new[,] { { 3, 2, 4, 5, 7 }, { -3, -2, -4, -5, -7 } }, 0, 0, 0);

            // Get the IEnumerator<T>
            var enumerator = darray.GetEnumerator();

            // Advance on step and check current value
            enumerator.MoveNext();
            Assert.AreEqual(3, enumerator.Current);

            // Invalidate the array
            invalidator(darray);

            // Current must *NOT* rhrow if invalidation is right after MoveNext
            // see http://msdn.microsoft.com/en-us/library/system.collections.ienumerator.aspx
            Assert.AreEqual(3, enumerator.Current);

            // Move the invalidated enumerator (InvalidOperationException expected)
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());

            // Reset the invalidated enumerator (InvalidOperationException expected)
            Assert.Throws<InvalidOperationException>(enumerator.Reset);
        }
    }
}
