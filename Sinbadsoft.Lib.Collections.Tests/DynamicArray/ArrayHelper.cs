// <copyright file="ArrayHelper.cs" company="Sinbadsoft">
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
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Helps building and printing arrays for testing purposes.
    /// </summary>
    internal class ArrayHelper
    {
        private Array result;

        /// <summary>
        /// Prevents a default instance of the ArrayHelper class from being created.
        /// User the factory method instead.
        /// </summary>
        /// <see cref="New"/>
        private ArrayHelper()
        {
        }

        /// <summary>
        /// Copies the given array to a new <see cref="DynamicArray{T}"/>.
        /// Each element is copied at the same indexes using the
        /// <see cref="DynamicArray{T}.Item"/> indexer.
        /// </summary>
        /// <param name="arr"> The array to convert to a dynamic array. </param>
        /// <typeparam name="T"> The type of the elements of the DynamicArray to create. </typeparam>
        /// <returns> The resulting new <see cref="DynamicArray{T}"/>. </returns>
        internal static DynamicArray<T> ToDynamic<T>(Array arr)
        {
            CopyToDynamicArrayAction<T> copyToAction = new CopyToDynamicArrayAction<T>(arr.Rank);
            RecursiveArrayEnumerator.Iterate(arr, copyToAction.Enumerate);
            return copyToAction.DArray;
        }

        internal static string SequenceToString<T>(T[] sequence)
        {
            StringBuilder sbuilder = new StringBuilder("(");
            for (int i = 0; i < sequence.Length; ++i)
            {
                sbuilder.Append(sequence[i]);
                if (i < sequence.Length - 1)
                {
                    sbuilder.Append(',');
                }
            }

            return sbuilder.Append(')').ToString();
        }

        internal static void Print<T>(DynamicArray<T> arr)
        {
            Console.Out.WriteLine(ToString(arr));
            Console.Out.WriteLine();
        }

        internal static string ToString<T>(DynamicArray<T> dynarray)
        {
            StringBuilder sbuilder = new StringBuilder();
            for (int i0 = 0; i0 < dynarray.GetCount(0); ++i0)
            {
                const char LEFT = '[', RIGHT = ']';
                sbuilder.Append(LEFT).Append(' ');
                for (int i1 = 0; i1 < dynarray.GetCount(1); ++i1)
                {
                    sbuilder.Append(LEFT);
                    string valstr = Convert.ToString(dynarray.GetValue(i0, i1), CultureInfo.InvariantCulture);
                    sbuilder.Append(string.IsNullOrEmpty(valstr.Trim()) ? "------" : valstr).Append(RIGHT);
                }

                sbuilder.Append(' ').Append(RIGHT).AppendLine();
            }

            return sbuilder.ToString();
        }

        internal static ArrayHelper New()
        {
            return new ArrayHelper();
        }

        internal TArray As<TArray>()
        {
            return (TArray)(object)this.result;
        }

        internal ArrayHelper FillWith(string prefix)
        {
            RecursiveArrayEnumerator.Iterate(this.result, new FillAction(prefix).Enumerate);
            return this;
        }

        internal ArrayHelper NewArray<T>(params int[] lengths)
        {
            this.result = Array.CreateInstance(typeof(T), lengths);
            return this;
        }

        private class CopyToDynamicArrayAction<T>
        {
            internal readonly DynamicArray<T> DArray;

            public CopyToDynamicArrayAction(int rank)
            {
                this.DArray = new DynamicArray<T>(rank);
            }

            public void Enumerate(Array arr, params int[] indexes)
            {
                this.DArray[indexes] = (T)arr.GetValue(indexes);
            }
        }

        private class FillAction
        {
            private readonly string prefix;

            public FillAction(string prefix)
            {
                this.prefix = prefix;
            }

            public void Enumerate(Array arr, params int[] indexes)
            {
                arr.SetValue(this.prefix + SequenceToString(indexes), indexes);
            }
        }
    }
}