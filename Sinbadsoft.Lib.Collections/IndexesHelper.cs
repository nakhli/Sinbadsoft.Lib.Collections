// <copyright file="IndexesHelper.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/01</date>

namespace Sinbadsoft.Lib.Collections
{
    using System;
    using System.Text;

    internal static class IndexesHelper
    {
        /// <summary>
        /// Creates a new array with values equal to those of <paramref name="a1"/> plus <paramref name="a2"/>.
        /// </summary>
        /// <param name="a1"> The left operand.</param>
        /// <param name="a2"> The right operand.</param>
        /// <returns> A new array containing the result.</returns>
        internal static int[] Add(int[] a1, int[] a2)
        {
            int[] result = Zero(a1.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                result[i] = a1[i] + a2[i];
            }

            return result;
        }

        /// <summary>
        /// Creates a new array with values equal to those of 
        /// <paramref name="a1"/> minus <paramref name="a2"/>.
        /// </summary>
        /// <param name="a1"> The left operand.</param>
        /// <param name="a2"> The right operand.</param>
        /// <returns> A new array containing the result.</returns>
        internal static int[] Substract(int[] a1, int[] a2)
        {
            int[] result = Zero(a1.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                result[i] = a1[i] - a2[i];
            }

            return result;
        }

        /// <summary> Creates a new array with values equal to those of <paramref name="arr"/> plus <paramref name="scalar"/>.</summary>
        /// <param name="arr"> The source array.</param>
        /// <param name="scalar"> The scalar value to add.</param>
        /// <returns> A new array containing the result.</returns>
        internal static int[] Add(int[] arr, int scalar)
        {
            int[] result = (int[])arr.Clone();
            for (int i = 0; i < result.Length; i++)
            {
                result[i] += scalar;
            }

            return result;
        }

        /// <summary> 
        /// Creates a new array with values equal to those of <paramref name="arr"/> multiplied by <paramref name="scalar"/>. 
        /// </summary>
        /// <param name="arr"> The source array.</param>
        /// <param name="scalar"> The scalar value to multiply by.</param>
        /// <returns> A new array containing the result.</returns>
        internal static int[] Multiply(int[] arr, int scalar)
        {
            int[] result = (int[])arr.Clone();
            for (int i = 0; i < result.Length; i++)
            {
                result[i] *= scalar;
            }

            return result;
        }

        internal static int[] Zero(int rank)
        {
            return new int[rank];
        }

        internal static int[] Clone(int[] arr)
        {
            return (int[])arr.Clone();
        }

        internal static int[] ProjectOnIthDim(int[] arr, int dim)
        {
            int[] result = Zero(arr.Length);
            result[dim] = arr[dim];
            return result;
        }

        internal static int[] TakeMinOnIthDim(int[] dest, int[] src, int dim)
        {
            if (dim >= 0 && dest[dim] > src[dim])
            {
                dest[dim] = src[dim];
            }

            return dest;
        }

        internal static bool Dec(int[] arr, int[] lowerbound, int[] max)
        {
            for (int dimension = arr.Length - 1; dimension >= 0; --dimension)
            {
                if (arr[dimension] > lowerbound[dimension] + 1)
                {
                    --arr[dimension];
                    return true;
                }

                arr[dimension] = max[dimension];
            }

            return false;
        }

        internal static bool Inc(int[] arr, int[] upperbound, int[] zero)
        {
            for (int dimension = arr.Length - 1; dimension >= 0; --dimension)
            {
                if (arr[dimension] < upperbound[dimension] - 1)
                {
                    ++arr[dimension];
                    return true;
                }

                arr[dimension] = zero[dimension];
            }

            return false;
        }

        internal static bool ElementsEqual(int[] a1, int[] a2)
        {
            for (int i = 0; i < a1.Length; ++i)
            {
                if (Equals(a1[i], a2[i]))
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        internal static bool Less(int[] a1, int[] a2)
        {
            for (int dimension = a1.Length - 1; dimension >= 0; --dimension)
            {
                if (a1[dimension] < a2[dimension])
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        internal static int[] MaxIndexes(Array arr)
        {
            int[] maxIndices = Zero(arr.Rank);
            for (int dim = 0; dim < arr.Rank; ++dim)
            {
                maxIndices[dim] = arr.GetLength(dim) - 1;
            }

            return maxIndices;
        }

        internal static int[] UpperBound(Array arr)
        {
            int[] lengths = Zero(arr.Rank);
            for (int dim = 0; dim < arr.Rank; ++dim)
            {
                lengths[dim] = arr.GetLength(dim);
            }

            return lengths;
        }
    }
}