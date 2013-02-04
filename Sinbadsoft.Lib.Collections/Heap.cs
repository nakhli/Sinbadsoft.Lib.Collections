// <copyright file="Heap.cs" company="Sinbadsoft">
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

    public class Heap<T>
    {
        private readonly IList<T> list;
        private readonly Func<T, T, int> comparer;

        public Heap() : this(Comparer<T>.Default.Compare)
        {
        }

        public Heap(IComparer<T> comparer) : this(new List<T>(), 0, comparer)
        {
        }

        public Heap(Func<T, T, int> comparer) : this(new List<T>(), 0, comparer)
        {
        }

        public Heap(IList<T> list, int count, IComparer<T> comparer) : this(list, count, comparer.Compare)
        {   
        }

        public Heap(IList<T> list, int count, Func<T, T, int> comparer)
        {
            this.comparer = comparer;
            this.list = list;
            this.Count = count;
            this.Heapify();
        }

        public int Count { get; private set; }

        public T PopRoot()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Empty heap.");
            }

            var root = this.list[0];
            this.SwapCells(0, this.Count - 1);
            this.Count--;
            this.HeapDown(0);
            return root;
        }

        public T PeekRoot()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Empty heap.");
            }

            return this.list[0];
        }

        public void Insert(T e)
        {
            if (this.Count >= this.list.Count)
            {
                this.list.Add(e);
            }
            else
            {
                this.list[this.Count] = e;
            }

            this.Count++;
            this.HeapUp(this.Count - 1);
        }

        public void Clear(bool clearUnderlyingList = false)
        {
            this.Count = 0;
            if (clearUnderlyingList)
            {
                this.list.Clear();
            }
        }

        private void Heapify()
        {
            for (int i = this.Parent(this.Count - 1); i >= 0; i--)
            {
                this.HeapDown(i);
            }
        }

        private void HeapUp(int i)
        {
            T elt = this.list[i];
            while (true)
            {
                int parent = this.Parent(i);
                if (parent < 0 || this.comparer(this.list[parent], elt) > 0)
                {
                    break;
                }

                this.SwapCells(i, parent);
                i = parent;
            }
        }

        private void HeapDown(int i)
        {
            while (true)
            {
                int lchild = this.LeftChild(i);
                if (lchild < 0)
                {
                    break;
                }

                int rchild = this.RightChild(i);

                int child = rchild < 0
                  ? lchild
                  : this.comparer(this.list[lchild], this.list[rchild]) > 0 ? lchild : rchild;

                if (this.comparer(this.list[child], this.list[i]) < 0)
                {
                    break;
                }

                this.SwapCells(i, child);
                i = child;
            }
        }

        private int Parent(int i)
        {
            return i <= 0 ? -1 : this.SafeIndex((i - 1) / 2);
        }

        private int RightChild(int i)
        {
            return this.SafeIndex((2 * i) + 2);
        }

        private int LeftChild(int i)
        {
            return this.SafeIndex((2 * i) + 1);
        }

        private int SafeIndex(int i)
        {
            return i < this.Count ? i : -1;
        }

        private void SwapCells(int i, int j)
        {
            T temp = this.list[i];
            this.list[i] = this.list[j];
            this.list[j] = temp;
        }
    }
}