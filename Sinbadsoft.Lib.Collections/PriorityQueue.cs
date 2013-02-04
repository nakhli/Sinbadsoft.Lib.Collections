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

        public PriorityQueue()
            : this(Comparer<T>.Default.Compare, DEFAULT_CAPACITY, null)
        {
        }

        public PriorityQueue(IEnumerable<T> collection)
            : this(Comparer<T>.Default.Compare, DEFAULT_CAPACITY, collection)
        {
        }

        public PriorityQueue(int capacity)
            : this(Comparer<T>.Default.Compare, capacity, null)
        {
        }

        public PriorityQueue(IComparer<T> comparer, IEnumerable<T> collection = null)
            : this(comparer.Compare, DEFAULT_CAPACITY, collection)
        {
        }

        public PriorityQueue(IComparer<T> comparer, int capacity = DEFAULT_CAPACITY)
            : this(comparer.Compare, capacity, null)
        {
        }

        public PriorityQueue(Func<T, T, int> comparer, IEnumerable<T> collection = null)
            : this(comparer, DEFAULT_CAPACITY, collection)
        {
        }

        public PriorityQueue(Func<T, T, int> comparer, int capacity = DEFAULT_CAPACITY)
            : this(comparer, capacity, null)
        {
        }

        protected PriorityQueue(Func<T, T, int> comparer, int capacity, IEnumerable<T> collection)
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

    public class PriorityQueue<TKey, TValue> : PriorityQueue<KeyValuePair<TKey, TValue>>
    {
        public PriorityQueue()
        {
        }

        public PriorityQueue(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : base(collection)
        {
        }

        public PriorityQueue(int capacity)
            : base(capacity)
        {
        }

        public PriorityQueue(IComparer<TKey> comparer, IEnumerable<KeyValuePair<TKey, TValue>> collection = null)
            : base((p1, p2) => comparer.Compare(p1.Key, p2.Key), collection)
        {
        }

        public PriorityQueue(IComparer<TKey> comparer, int capacity = DEFAULT_CAPACITY)
            : base((p1, p2) => comparer.Compare(p1.Key, p2.Key), capacity)
        {
        }

        public PriorityQueue(Func<TKey, TKey, int> comparer, IEnumerable<KeyValuePair<TKey, TValue>> collection = null)
            : base((p1, p2) => comparer(p1.Key, p2.Key), collection)
        {
        }

        public PriorityQueue(Func<TKey, TKey, int> comparer, int capacity = DEFAULT_CAPACITY)
            : base((p1, p2) => comparer(p1.Key, p2.Key), capacity)
        {
        }

        public void Enqueue(TKey key, TValue value)
        {
            this.Enqueue(new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}
