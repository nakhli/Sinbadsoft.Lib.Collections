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
        public const int DEFAULT_CAPACITY = 16;
        private readonly Heap<T> heap;
        private readonly List<T> list;

        public PriorityQueue()
            : this(Comparer<T>.Default, DEFAULT_CAPACITY, null)
        {
        }

        public PriorityQueue(IEnumerable<T> collection)
            : this(Comparer<T>.Default, DEFAULT_CAPACITY, collection)
        {
        }

        public PriorityQueue(IComparer<T> comparer, IEnumerable<T> collection = null)
            : this(comparer, DEFAULT_CAPACITY, collection)
        {
        }

        public PriorityQueue(int capacity)
            : this(Comparer<T>.Default, capacity, null)
        {
        }

        public PriorityQueue(IComparer<T> comparer, int capacity = DEFAULT_CAPACITY)
            : this(comparer, capacity, null)
        {
        }

        private PriorityQueue(IComparer<T> comparer, int capacity, IEnumerable<T> collection)
        {
            this.list = new List<T>(capacity);
            this.heap = new Heap<T>(this.list, 0, comparer);
            if (collection != null)
            {
                foreach (var element in collection)
                {
                    this.Enqueue(element);
                }
            }
        }

        public int Count
        {
            get { return this.heap.Count; }
        }

        public T Peek()
        {
            return this.heap.PeekRoot();
        }

        public void Enqueue(T e)
        {
            this.heap.Insert(e);
        }

        public T Dequeue()
        {
            return this.heap.PopRoot();
        }

        public void Clear()
        {
            this.heap.Clear(true);
        }

        public void TrimExcess()
        {
            this.list.TrimExcess();
        }
    }
}
