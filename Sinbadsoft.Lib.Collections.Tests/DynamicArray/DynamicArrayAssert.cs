// <copyright file="DynamicArrayAssert.cs" company="Sinbadsoft">
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

    public static class DynamicArrayAssert
    {
        public static void AreElementsDefault<T>(DynamicArray<T> dynarray, int[] lengths, params int[] position)
        {
            Array empty = ArrayHelper.New().NewArray<T>(lengths).As<Array>();
            Included(dynarray, empty, position);
        }

        /// <summary>
        /// Asserts the two arrays have the same elements.
        /// Checks count equality using <see cref="CountsEqual{T}"/>.
        /// Elements are get from the <see cref="DynamicArray{T}"/> using its indexer.
        /// </summary>
        /// <typeparam name="T"> The type of the elements int the <see cref="DynamicArray{T}"/>. </typeparam>
        /// <param name="dynarray"> The <see cref="DynamicArray{T}"/> being checked. </param>
        /// <param name="arr"> The array containing the actual elements. </param>
        public static void AreElementsEqual<T>(DynamicArray<T> dynarray, Array arr)
        {
            CountsEqual(dynarray, IndexesHelper.UpperBound(arr));
            Included(dynarray, arr, IndexesHelper.Zero(arr.Rank));
        }

        public static void CountsEqual<T>(DynamicArray<T> dynarray, params int[] counts)
        {
            for (int i = 0; i < dynarray.Rank; ++i)
            {
                Assert.AreEqual(counts[i], dynarray.GetCount(i));
            }
        }

        public static void Included<T>(DynamicArray<T> dynarray, Array arr, params int[] position)
        {
            RecursiveArrayEnumerator.Iterate(arr, new Inclusion<T>(dynarray, position).Enumerate);
        }

        public static void Included<T>(DynamicArray<T> dynarray, Array arr, int[] position, int[] lower, int[] upper)
        {
            RecursiveArrayEnumerator.Iterate(arr, lower, upper, new Inclusion<T>(dynarray, position, lower).Enumerate, false);
        }

        private abstract class EnumerateAction<T>
        {
            protected readonly DynamicArray<T> DynArray;

            protected EnumerateAction(DynamicArray<T> dynarray)
            {
                this.DynArray = dynarray;
            }

            public abstract void Enumerate(Array arr, params int[] indexes);
        }

        private class Inclusion<T> : EnumerateAction<T>
        {
            private readonly int[] position;

            public Inclusion(DynamicArray<T> darr, int[] position, int[] offset) : base(darr)
            {
                this.position = IndexesHelper.Substract(position, offset);
            }

            public Inclusion(DynamicArray<T> darr, int[] position)
                : base(darr)
            {
                this.position = position;
            }

            public override void Enumerate(Array arr, int[] indexes)
            {
                Assert.AreEqual((T)arr.GetValue(indexes), this.DynArray[IndexesHelper.Add(indexes, this.position)]);
            }
        }
    }
}