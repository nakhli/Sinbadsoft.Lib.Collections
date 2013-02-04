// <copyright file="HeapSort.cs" company="Sinbadsoft">
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

    public class HeapSort<T>
    {
        public static void Sort(IList<T> list, IComparer<T> comparer)
        {
            var heap = new Heap<T>(list, list.Count, comparer);
            while (heap.Count > 0)
            {
                heap.PopRoot();
            }
        }
    }
}