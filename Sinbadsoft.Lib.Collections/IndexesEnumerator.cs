// <copyright file="IndexesEnumerator.cs" company="Sinbadsoft">
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
    using System.Collections;
    using System.Collections.Generic;

    internal class IndexesEnumerator : IEnumerable<int[]>, IEnumerator<int[]>
    {
        private readonly int[] beginIdxs;
        private readonly int[] currentIdxs;
        private readonly int[] endIdxs;
        private readonly bool reverse;

        public IndexesEnumerator(int[] upperBound) : this(upperBound, IndexesHelper.Zero(upperBound.Length), false)
        {
        }

        public IndexesEnumerator(int[] upperBound, int[] lowerBound) : this(upperBound, lowerBound, false)
        {
        }

        public IndexesEnumerator(int[] endIndexes, int[] beginIndexes, bool reverse)
        {
            if (endIndexes.Length != beginIndexes.Length)
            {
                throw new ArgumentException("UpperBound and LowerBound arrays must have the same length.");
            }

            this.endIdxs = (int[])endIndexes.Clone();
            this.beginIdxs = (int[])beginIndexes.Clone();
            this.reverse = reverse;

            if ((reverse && IndexesHelper.Less(this.endIdxs, this.beginIdxs))
                || (!reverse && IndexesHelper.Less(this.beginIdxs, this.endIdxs)))
            {
                this.currentIdxs = (int[])beginIndexes.Clone();
                if (reverse)
                {
                    ++this.currentIdxs[this.currentIdxs.Length - 1];
                }
                else
                {
                    --this.currentIdxs[this.currentIdxs.Length - 1];
                }
            }
            else
            {
                // NOTE do pervent enumeration
                this.currentIdxs = this.endIdxs;
            }
        }

        public int[] Current
        {
            get
            {
                return this.currentIdxs;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.currentIdxs;
            }
        }

        public static IndexesEnumerator Enumerate(Array arr)
        {
            return Enumerate(arr, false);
        }

        public static IndexesEnumerator Enumerate(Array arr, bool reverse)
        {
            return new IndexesEnumerator(IndexesHelper.UpperBound(arr), IndexesHelper.Zero(arr.Rank), reverse);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerator<int[]> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            return this.reverse
                ? IndexesHelper.Dec(this.currentIdxs, this.endIdxs, this.beginIdxs)
                : IndexesHelper.Inc(this.currentIdxs, this.endIdxs, this.beginIdxs);
        }

        public void Reset()
        {
            Array.Copy(this.beginIdxs, this.currentIdxs, this.currentIdxs.Length);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}