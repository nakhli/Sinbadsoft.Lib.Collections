// <copyright file="DynamicArray.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <author>Ludovic CONTAMINE (designed the Resize algorithm)</author>
// <email>ludovic.contamine@free.fr</email>
// <date>2009/03/01</date>

namespace Sinbadsoft.Lib.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary> Dynamic Multidimensional Array. </summary>
    /// <typeparam name="T"> The type of the keys in the array. </typeparam>
    [DebuggerDisplay("{ToArray()}")]
    [DebuggerTypeProxy(typeof(DynamicArrayDebugView<>))]
    public class DynamicArray<T> : ICollection, IEnumerable<T>
    {
        /// <summary> The default capacity of the underlying buffer. </summary>
        private const int DEFAULTCAPACITY = 16;

        /// <summary> The array counts of each dimension. </summary>
        private readonly int[] counts;

        /// <summary> The underlying buffer array. </summary>
        private Array buffer;

        private int changeCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicArray{T}"/> class. If a capcity is omitted the 
        /// <see cref="DEFAULTCAPACITY"/> is used for the missing dimension capacity.
        /// </summary>
        /// <param name="rank"> The dynamic array rank. </param>
        /// <param name="capacities"> The capacities to be used for the underlying buffer. </param>
        public DynamicArray(int rank, params int[] capacities)
        {
            this.counts = IndexesHelper.Zero(rank);
            int[] usedCapacities = IndexesHelper.Zero(rank);
            for (int i = 0; i < rank; ++i)
            {
                usedCapacities[i] = (i < capacities.Length ? capacities[i] : DEFAULTCAPACITY);
            }

            // NOTE will throw an ArgumentException if rank<1
            this.buffer = Array.CreateInstance(typeof(T), usedCapacities);
        }
        
        /// <summary> Gets the number of elements contained in the <see cref="ICollection"/>. </summary>
        /// <returns> The number of elements contained in the <see cref="ICollection"/>. </returns>
        public int Count
        {
            get
            {
                int length = 1;
                for (int dimension = 0; dimension < this.Rank; ++dimension)
                {
                    length *= this.GetCount(dimension);
                }

                return length;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="ICollection"/> 
        /// is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if access to the <see cref="ICollection"/> is synchronized (thread safe); 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary> Gets the number of dimensions. </summary>
        public int Rank
        {
            get
            {
                return this.counts.Length;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
        /// </summary>
        /// <returns> An object that can be used to synchronize access to the <see cref="ICollection"/>. </returns>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary> Gets fail fast counter for enumerator invalidation. </summary>
        internal int ChangeCount
        {
            get { return this.changeCount; }
        }

        /// <summary> Gets a copy of the <see cref="DynamicArray{T}"/> dimensions' counts. </summary>
        internal int[] Counts
        {
            get { return IndexesHelper.Clone(this.counts); }
        }

        /// <summary> Gets or sets the value associated with the specified indexes. </summary>
        /// <param name="indexes">The position where to get or set the element.</param>
        public T this[params int[] indexes]
        {
            get
            {
                return this.GetValue(indexes);
            }

            set
            {
                this.SetValue(value, indexes);
            }
        }

        /// <summary>
        /// Converts a <see cref="DynamicArray{T}"/> to a system array of a given dimension.
        /// </summary>
        /// <param name="darr"> The <see cref="DynamicArray{T}"/> to convert.</param>
        /// <returns> A one dimensional array holding all the <see cref="DynamicArray{T}"/> elements. </returns>
        /// <remarks> This is convenient operator. The implementation performs a cast of 
        /// <see cref="ToArray"/> to the desired array type.</remarks>
        public static explicit operator T[](DynamicArray<T> darr)
        {
            return (T[])darr.ToArray();
        }

        /// <summary>
        /// Converts a <see cref="DynamicArray{T}"/> to a system array of a given dimension.
        /// </summary>
        /// <param name="darr"> The <see cref="DynamicArray{T}"/> to convert.</param>
        /// <returns> A two dimensional array holding all the <see cref="DynamicArray{T}"/> elements. </returns>
        /// <remarks> This is convenient operator. The implementation performs a cast of 
        /// <see cref="ToArray"/> to the desired array type.</remarks>
        public static explicit operator T[,](DynamicArray<T> darr)
        {
            return (T[,])darr.ToArray();
        }

        /// <summary>
        /// Converts a <see cref="DynamicArray{T}"/> to a system array of a given dimension.
        /// </summary>
        /// <param name="darr"> The <see cref="DynamicArray{T}"/> to convert.</param>
        /// <returns> A three dimensional array holding all the <see cref="DynamicArray{T}"/> elements. </returns>
        /// <remarks> This is convenient operator. The implementation performs a cast of 
        /// <see cref="ToArray"/> to the desired array type.</remarks>
        public static explicit operator T[,,](DynamicArray<T> darr)
        {
            return (T[,,])darr.ToArray();
        }

        /// <summary>
        /// Converts a <see cref="DynamicArray{T}"/> to a system array of a given dimension.
        /// </summary>
        /// <param name="darr"> The <see cref="DynamicArray{T}"/> to convert.</param>
        /// <returns> A four dimensional array holding all the <see cref="DynamicArray{T}"/> elements. </returns>
        /// <remarks> This is convenient operator. The implementation performs a cast of 
        /// <see cref="ToArray"/> to the desired array type.</remarks>
        public static explicit operator T[,,,](DynamicArray<T> darr)
        {
            return (T[,,,])darr.ToArray();
        }

        /// <summary>
        /// Converts a <see cref="DynamicArray{T}"/> to <see cref="Array"/>.
        /// </summary>
        /// <param name="darr"> The <see cref="DynamicArray{T}"/> to convert.</param>
        /// <returns> An <see cref="Array"/> holding all the <see cref="DynamicArray{T}"/> elements. </returns>
        /// <remarks> This is convenient operator. The implementation performs a cast of 
        /// <see cref="ToArray"/> to <see cref="Array"/>.</remarks>
        public static explicit operator Array(DynamicArray<T> darr)
        {
            return darr.ToArray();
        }

        /// <summary>
        /// Sets a range of elements in the <see cref="DynamicArray{T}"/> to zero, to <see langword="false"/> 
        /// or to <see langword="null"/>, depending on <typeparamref name="T"/>.
        /// </summary>
        public void Clear()
        {
            Array.Clear(this.buffer, 0, this.buffer.Length);
            Array.Clear(this.counts, 0, this.Rank);
            ++this.changeCount;
        }

        /// <summary>
        /// Determines whether the <see cref="DynamicArray{T}"/> contains a specific value. Complexity is <em>O(n)</em>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="DynamicArray{T}"/>. </param>
        /// <returns> Returns <see langword="true"/> if <paramref name="item"/> is found in the 
        /// <see cref="DynamicArray{T}"/> otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(T item)
        {
            foreach (T t in this)
            {
                if (Equals(t, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary> Strongly typed implementation of <see cref="ICollection{T}.CopyTo"/>. </summary>
        /// <param name="darray"> The <see cref="DynamicArray{T}"/> where to copy the values. </param>
        /// <param name="indexes">The zero-based indexes offset in darray at which copying begins. </param>
        public void CopyTo(DynamicArray<T> darray, params int[] indexes)
        {
            DynamicArrayEnumerator<T> enumerator = this.GetEnumerator();
            foreach (T elt in enumerator)
            {
                darray.SetValue(elt, IndexesHelper.Add(enumerator.GetIndexes(), indexes));
            }
        }

        /// <summary> 
        /// Resizes the <see cref="DynamicArray{T}"/> by adding each ith given value to the corresponding ith dimension.
        /// </summary>
        /// <param name="addedCounts"> The amounts to add to each dimension count. </param>
        public void Extend(params int[] addedCounts)
        {
            this.CheckIndexesLength(addedCounts);
            this.Resize(IndexesHelper.Add(this.counts, addedCounts));
        }

        /// <summary>
        /// Resizes the given dimension of the <see cref="DynamicArray{T}"/> by adding the given value to its 
        /// corresponding count.
        /// </summary>
        /// <param name="dimension"> The <see cref="DynamicArray{T}"/> dimension to enlarge.</param>
        /// <param name="addedCount"> The value to add to the <see cref="DynamicArray{T}"/> 
        /// <paramref name="dimension"/>'s count.</param>
        public void ExtendDim(int dimension, int addedCount)
        {
            this.ResizeDim(dimension, this.GetCount(dimension) + addedCount);
        }

        /// <summary> Gets the capacity of the given dimension. </summary>
        /// <param name="dimension"> The dimension of which the capcity is returned.</param>
        /// <returns> The <see cref="DynamicArray{T}"/> <paramref name="dimension"/>'s capacity. </returns>
        public int GetCapacity(int dimension)
        {
            return this.buffer.GetLength(dimension);
        }

        /// <summary> Gets the <see cref="DynamicArray{T}"/> count for the given dimension. </summary>
        /// <param name="dimension"> One of the a <see cref="DynamicArray{T}"/>'s dimensions. </param>
        /// <returns> The given dimension length. </returns>
        public int GetCount(int dimension)
        {
            return this.counts[dimension];
        }

        /// <summary>
        /// Gets a <see cref="DynamicArrayEnumerator{T}"/> that iterates through the <see cref="DynamicArray{T}"/>.
        /// </summary>
        /// <returns> A <see cref="DynamicArrayEnumerator{T}"/> enumerator object that can be used to iterate through 
        /// the <see cref="DynamicArray{T}"/>. </returns>
        public DynamicArrayEnumerator<T> GetEnumerator()
        {
            return new DynamicArrayEnumerator<T>(this);
        }

        /// <summary>
        /// Gets the value at the specified position in the <see cref="DynamicArray{T}"/>. The indexes are specified 
        /// as an array of integers.
        /// </summary>
        /// <param name="indexes"> A one-dimensional array of integers that represent the indexes of the element 
        /// to get. </param>
        /// <returns> The value at the specified position in the <see cref="DynamicArray{T}"/>. </returns>
        public T GetValue(params int[] indexes)
        {
            return (T)this.buffer.GetValue(indexes);
        }

        /// <summary> 
        /// Inserts the given <see cref="Array"/> within the given dimension and at the given position. 
        /// </summary>
        /// <param name="arr"> An array to insert in the <see cref="DynamicArray{T}"/>. 
        /// It must be not <see langword="null"/> and it must have a rank equal to <see cref="DynamicArray{T}.Rank"/>.
        /// </param>
        /// <param name="dimension"> The dimension where the elements of the <paramref name="arr"/> will be inserted.
        /// </param>
        /// <param name="indexes"> The position where the first element of the <paramref name="arr"/> will be inserted.
        /// </param>
        /// <exception cref="RankException"> If <paramref name="arr"/> has a rank not equal to 
        /// <see cref="DynamicArray{T}.Rank"/>.</exception>
        /// <exception cref="ArgumentException"> If <paramref name="arr"/> has one of its lengths equal to zero. 
        /// </exception>
        /// <exception cref="NullReferenceException"> If arr is <see langword="null"/></exception>
        public void Insert(Array arr, int dimension, params int[] indexes)
        {
            this.CheckIndexesLength(indexes);

            if (arr.Rank != this.Rank)
            {
                throw new RankException("Inserted array must have the same rank as this array");
            }

            for (int dim = 0; dim < this.Rank; ++dim)
            {
                if (arr.GetLength(dim) > 0)
                {
                    continue;
                }

                throw new ArgumentException("Inserted array must have non null lengths.", "arr");
            }

            int[] oldCounts = IndexesHelper.Clone(this.counts);
            int dataCount = arr.GetLength(dimension);
            int[] lastIndices = IndexesHelper.Add(indexes, IndexesHelper.MaxIndexes(arr));
            lastIndices[dimension] = Math.Max(oldCounts[dimension], indexes[dimension]) + dataCount - 1;

            this.UpdateCountsAndCapacities(lastIndices);

            int[] rangeLowerBound = IndexesHelper.Add(indexes, -1);
            if (indexes[dimension] < oldCounts[dimension])
            {
                int[] rangeMax = IndexesHelper.Add(IndexesHelper.MaxIndexes(arr), indexes);
                rangeMax[dimension] = oldCounts[dimension] - 1;
                IndexesEnumerator rangeToMove = new IndexesEnumerator(rangeLowerBound, rangeMax, true);
                foreach (int[] source in rangeToMove)
                {
                    int[] destination = IndexesHelper.Clone(source);
                    destination[dimension] += dataCount;
                    this.buffer.SetValue(this.buffer.GetValue(source), destination);
                }
            }

            foreach (int[] source in IndexesEnumerator.Enumerate(arr))
            {
                int[] destination = IndexesHelper.Add(source, indexes);
                this.buffer.SetValue(arr.GetValue(source), destination);
            }

            ++this.changeCount;
        }

        /// <summary> Resizes the <see cref="DynamicArray{T}"/> to the given new counts. </summary>
        /// <param name="newCounts"> The new lengths of the <see ref="DynamicArray{T}"/>. </param>
        /// <remarks> If the <paramref name="newCounts"/> ith element is larger than the ith count, the new added 
        /// elements are set to <c>default(<typeparamref name="T"/>)</c>. If <paramref name="newCounts"/> ith element
        /// is less then the ith count, the <see ref="DynamicArray{T}"/> is shrank to fit to the new count value,
        /// and the removed elements will be lost. </remarks>
        public void Resize(params int[] newCounts)
        {
            this.CheckIndexesLength(newCounts);

            if (IndexesHelper.ElementsEqual(this.counts, newCounts))
            {
                return;
            }

            int[] oldCounts = IndexesHelper.Clone(this.counts);
            this.UpdateCountsAndCapacities(IndexesHelper.Add(newCounts, -1));
            for (int dim = 0; dim < this.Rank; ++dim)
            {
                // NOTE When this.buffer is shrank dismissed values are reset to default(T)
                IndexesEnumerator ienum = new IndexesEnumerator(
                    IndexesHelper.TakeMinOnIthDim(oldCounts, newCounts, dim - 1),
                    IndexesHelper.ProjectOnIthDim(newCounts, dim));
                foreach (int[] indexes in ienum)
                {
                    this.buffer.SetValue(default(T), indexes);
                }
            }

            Array.Copy(newCounts, this.counts, this.counts.Length);
            ++this.changeCount;
        }

        /// <summary> Resizes the given dimension to the given new count. </summary>
        /// <param name="dimension"> The <see ref="DynamicArray{T}"/>'s dimension to resize. </param>
        /// <param name="newCount"> The new length of the <see ref="DynamicArray{T}"/>'s dimension 
        /// <paramref name="dimension"/>. </param>
        /// <remarks> If the given <paramref name="newCount"/> is larger than the current count, the new added elements 
        /// are set to <c>default(<typeparamref name="T"/>)</c>. If <paramref name="newCount"/> is less then the current
        /// count, the <see ref="DynamicArray{T}"/> is shrank to fit to the new count value, and the removed elements 
        /// will be lost. </remarks>
        public void ResizeDim(int dimension, int newCount)
        {
            int[] newCounts = (int[])this.counts.Clone();
            newCounts[dimension] = newCount;
            this.Resize(newCounts);
        }

        /// <summary>
        /// Sets the value at the specified position in the <see cref="DynamicArray{T}"/>. The indexes are specified as 
        /// an array of integers.
        /// </summary>
        /// <param name="value"> The new value to set at the specified position in the <see cref="DynamicArray{T}"/>. 
        /// </param>
        /// <param name="indexes"> A one-dimensional array of integers that represent the indexes of the element to set. 
        /// </param>
        public void SetValue(T value, params int[] indexes)
        {
            this.CheckIndexesLength(indexes);
            this.UpdateCountsAndCapacities(indexes);
            this.buffer.SetValue(value, indexes);
            ++this.changeCount;
        }

        /// <summary>
        /// Copies the elements of the <see cref="DynamicArray{T}"/> to a new array with rank 
        /// <see cref="DynamicArray{T}.Rank"/> and with lengthes equal to <see cref="DynamicArray{T}"/> counts.
        /// </summary>
        /// <returns> An array containing copies of the elements of the<see cref="DynamicArray{T}"/>. </returns>
        public Array ToArray()
        {
            Array data = Array.CreateInstance(typeof(T), this.counts);
            DynamicArrayEnumerator<T> dynenum = new DynamicArrayEnumerator<T>(this);
            foreach (T elt in dynenum)
            {
                data.SetValue(elt, dynenum.GetIndexes());
            }

            return data;
        }

        void ICollection.CopyTo(Array arr, int index)
        {
            if (this.Rank > 1)
            {
                throw new RankException("Only single dimensional arrays are supported for the requested action.");
            }

            int i = index;
            foreach (T t in this)
            {
                arr.SetValue(t, i);
                ++i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DynamicArrayEnumerator<T>(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new DynamicArrayEnumerator<T>(this);
        }

        private void CheckIndexesLength(ICollection<int> indexes)
        {
            if (indexes.Count != this.Rank)
            {
                throw new ArgumentException(
                        string.Format(
                                CultureInfo.CurrentCulture,
                                "Input array is of incorrect length; expecting {0}, got {1}.",
                                this.Rank,
                                indexes.Count));
            }
        }

        /// <summary>
        /// Ensures the given indexes falls into the lengths and capacities of the underlying array. If not, the 
        /// capacities and lengths are updated accordingly. A new underlyting array may be allocated in order to adapt
        /// capacities.
        /// </summary>
        /// <param name="indexes"> The indexes that must fall within the array. </param>
        private void UpdateCountsAndCapacities(int[] indexes)
        {
            int[] newCounts = IndexesHelper.Zero(this.Rank);
            int[] newCapacities = IndexesHelper.Zero(this.Rank);
            bool newArrayNeeded = false;
            bool newCountsNeeded = false;
            for (int dimension = 0; dimension < this.Rank; ++dimension)
            {
                int requestedIndex = indexes[dimension];
                int currentCapacity = this.GetCapacity(dimension);
                int currentCount = this.GetCount(dimension);
                newCounts[dimension] = currentCount;
                newCapacities[dimension] = currentCapacity;
                if (requestedIndex < currentCount)
                {
                    continue;
                }

                newCountsNeeded = true;
                newCounts[dimension] = requestedIndex + 1;
                if (newCounts[dimension] <= currentCapacity)
                {
                    continue;
                }

                newCapacities[dimension] = Math.Max(2 * currentCapacity, newCounts[dimension]);
                newArrayNeeded = true;
            }

            if (!newCountsNeeded)
            {
                return;
            }

            if (newArrayNeeded)
            {
                Array newData = Array.CreateInstance(typeof(T), newCapacities);
                IndexesEnumerator ienum = new IndexesEnumerator(IndexesHelper.UpperBound(this.buffer));
                foreach (int[] idx in ienum)
                {
                    newData.SetValue(this.buffer.GetValue(idx), idx);
                }

                this.buffer = newData;
            }

            // NOTE New lengths are set after allocation to avoid lengths corruption on failure.
            Array.Copy(newCounts, this.counts, this.counts.Length);
        }
    }
}