// <copyright file="PriorityQueue.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2011-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2011/01/08</date>
namespace Sinbadsoft.Lib.Collections
{
    using System.Collections.Generic;

    public class PriorityQueue<T>
    {
        private readonly Heap<T> heap;

        public PriorityQueue() : this(Comparer<T>.Default)
        {
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            this.heap = new Heap<T>(new List<T>(), 0, comparer);
        }

        public int Size
        {
            get { return this.heap.Count; }
        }

        public T Top()
        {
            return this.heap.PeekRoot();
        }

        public void Push(T e)
        {
            this.heap.Insert(e);
        }

        public T Pop()
        {
            return this.heap.PopRoot();
        }
    }
}
