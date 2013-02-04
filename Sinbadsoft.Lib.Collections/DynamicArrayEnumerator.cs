// <copyright file="DynamicArrayEnumerator.cs" company="Sinbadsoft">
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

    /// <summary> Enumerator for <see cref="DynamicArray{T}"/> classes. </summary>
    /// <typeparam name="T"> The type of the elements of the dynamic array. </typeparam>
    public class DynamicArrayEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private DynamicArray<T> darray;
        private IEnumerator<int[]> indexesEnumerator;
        private int initialChangeCount;
        private bool valid;
        private T current;
        
        internal DynamicArrayEnumerator(DynamicArray<T> darray)
        {
            this.indexesEnumerator = new IndexesEnumerator(darray.Counts);
            this.darray = darray;
            this.initialChangeCount = darray.ChangeCount;
            this.valid = true;
        }

        /// <summary> Gets the element in the collection at the current position of the enumerator. </summary>
        /// <returns> The element in the collection at the current position of the enumerator. </returns>
        public T Current
        {
            get { return this.current; } 
        }

        /// <summary> Gets the current element in the collection. </summary>
        /// <returns> The current element in the collection. </returns>
        /// <exception cref="InvalidOperationException">The enumerator is positioned before the first element of the 
        /// collection or after the last element. </exception>
        object IEnumerator.Current
        {
            get { return this.current; }
        }

        /// <summary> 
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        /// <returns> A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection. </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        /// <summary> Returns the current indexes position. </summary>
        /// <returns> The indexes position of the next element.</returns>
        public int[] GetIndexes()
        {
            return this.indexesEnumerator.Current;
        }

        /// <summary> Advances the enumerator to the next element of the collection. </summary>
        /// <returns> Returns <see langword="true"/> if the enumerator was successfully advanced to the next element; 
        /// <see langword="false"/> if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The collection was modified after the enumerator was created.
        /// </exception>
        public bool MoveNext()
        {
            this.CheckValidity();
            if (this.indexesEnumerator.MoveNext())
            {
                this.current = this.darray.GetValue(this.indexesEnumerator.Current);
                return true;
            }

            return false;
        }

        /// <summary> 
        /// Sets the enumerator to its initial position, which is before the first element in the collection. 
        /// </summary>
        /// <exception cref="InvalidOperationException"> The collection was modified after the enumerator was created. 
        /// </exception>
        public void Reset()
        {
            this.CheckValidity();
            this.indexesEnumerator.Reset();
        }

        /// <summary> Returns an enumerator that iterates through a collection. </summary>
        /// <returns> An <see cref="IEnumerator"/> object that can be used to iterate through the collection. 
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        /// <summary> Implements the dispose pattern. </summary>
        /// <param name="disposing"> Indicates if the object is being disposed. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            this.valid = false;
            this.darray = null;
            if (this.indexesEnumerator != null)
            {
                this.indexesEnumerator.Dispose();
            }

            this.indexesEnumerator = null;
        }

        private void CheckValidity()
        {
            if (this.valid && this.initialChangeCount == this.darray.ChangeCount)
            {
                return;
            }

            this.valid = false;
            throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
        }
    }
}