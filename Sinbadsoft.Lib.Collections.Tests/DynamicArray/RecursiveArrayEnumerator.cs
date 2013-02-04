// <copyright file="RecursiveArrayEnumerator.cs" company="Sinbadsoft">
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

    /// <summary>
    /// Helper class for enumerating over an array of any dimension.
    /// This implementation uses a recursive algorithm (n-1 recursive calls where
    /// n is the given array dimension).
    /// </summary>
    public static class RecursiveArrayEnumerator
    {
        public static void Iterate(Array arr, Action<Array, int[]> action)
        {
            Iterate(arr, IndexesHelper.Zero(arr.Rank), IndexesHelper.UpperBound(arr), action, false);
        }

        public static void Iterate(Array arr, int[] startIndexes, int[] endIndexes, Action<Array, int[]> action, bool reverse)
        {
            if (startIndexes.Length != endIndexes.Length || (arr != null && arr.Rank != startIndexes.Length))
            {
                throw new ArgumentException(
                        "Indexes must have the same length equal to input array rank.", "startIndexes");
            }

            Iterate(arr, 0, IndexesHelper.Zero(startIndexes.Length), startIndexes, endIndexes, action, reverse);
        }

        private static void Iterate(
                Array arr,
                int dim,
                int[] indexes,
                int[] startIndexes,
                int[] endIndexes,
                Action<Array, int[]> action,
                bool reverse)
        {
            if (reverse)
            {
                for (int i = startIndexes[dim]; i > endIndexes[dim]; --i)
                {
                    LoopBody(arr, dim, indexes, startIndexes, endIndexes, action, true, i);
                }
            }
            else
            {
                for (int i = startIndexes[dim]; i < endIndexes[dim]; ++i)
                {
                    LoopBody(arr, dim, indexes, startIndexes, endIndexes, action, false, i);
                }
            }
        }

        private static void LoopBody(
                Array arr,
                int dim,
                int[] indexes,
                int[] startIndexes,
                int[] endIndexes,
                Action<Array, int[]> action,
                bool reverse,
                int i)
        {
            indexes[dim] = i;
            if (dim < indexes.Length - 1)
            {
                Iterate(arr, dim + 1, indexes, startIndexes, endIndexes, action, reverse);
            }
            else
            {
                action(arr, indexes);
            }
        }
    }
}