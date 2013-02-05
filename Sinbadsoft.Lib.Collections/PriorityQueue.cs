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
    using System;
    using System.Collections.Generic;

    public class PriorityQueue<T>
    {
        public const int DEFAULT_CAPACITY = 16;
        private readonly Heap<T> heap;
        private readonly List<T> list;

        public PriorityQueue(IEnumerable<T> collection)
            : this((Func<T, T, int>)null, collection)
        {
        }

        public PriorityQueue(int capacity)
            : this((Func<T, T, int>)null, null, capacity)
        {
        }

        public PriorityQueue(Func<T, T, int> comparer, int capacity)
            : this(comparer, null, capacity)
        {
        }

        public PriorityQueue(IComparer<T> comparer, int capacity)
            : this(comparer, null, capacity)
        {
        }

        public PriorityQueue(IComparer<T> comparer, IEnumerable<T> collection = null, int capacity = DEFAULT_CAPACITY)
            : this(comparer.Compare, collection, capacity)
        {
        }        

        public PriorityQueue(Func<T, T, int> comparer = null, IEnumerable<T> collection = null, int capacity = DEFAULT_CAPACITY)
        {
            this.list = new List<T>(capacity);
            this.heap = new Heap<T>(this.list, 0, comparer ?? Comparer<T>.Default.Compare);
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

    public class PriorityQueue<TKey, TValue> : PriorityQueue<KeyValuePair<TKey, TValue>>
    {
        public PriorityQueue(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : this((Func<TKey, TKey, int>)null, collection)
        {
        }

        public PriorityQueue(int capacity)
            : this((Func<TKey, TKey, int>)null, null, capacity)
        {
        }

        public PriorityQueue(Func<TKey, TKey, int> comparer, int capacity)
            : this(comparer, null, capacity)
        {
        }

        public PriorityQueue(IComparer<TKey> comparer, int capacity)
            : this(comparer, null, capacity)
        {
        }

        public PriorityQueue(IComparer<TKey> comparer, IEnumerable<KeyValuePair<TKey, TValue>> collection = null, int capacity = DEFAULT_CAPACITY)
            : this(comparer.Compare, collection, capacity)
        {
        }

        public PriorityQueue(Func<TKey, TKey, int> comparer = null, IEnumerable<KeyValuePair<TKey, TValue>> collection = null, int capacity = DEFAULT_CAPACITY)
            : base((p1, p2) => (comparer ?? Comparer<TKey>.Default.Compare)(p1.Key, p2.Key), collection, capacity)
        {
        }

        public void Enqueue(TKey key, TValue value)
        {
            this.Enqueue(new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}
