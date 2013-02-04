// <copyright file="PriorityQueueTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2013/02/03</date>
namespace Sinbadsoft.Lib.Collections.Tests.Heap
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class PriorityQueueTests
    {
        [Test]
        public void IntDataWithRepetitionsWithDefaultComparer()
        {
            var data = new[] { 125, 4, 5, 234, 6, 9076, 324, 2, 457, 928, 2, 123, 56, 73562, 770892, 452179, 125 };
            PushAllItemsThenPopAllAndCheckOrder(data);
        }

        [Test]
        public void StringDataWithRepetitionsWithDefaultComparer()
        {
            var data = new[] { "A" + 125, "A" + 4, "A" + 5, "A" + 234, "A" + 6, "A" + 9076, "A" + 324, "A" + 2, "A" + 457, "A" + 928, "A" + 2, "A" + 123, "A" + 56, "A" + 73562, "A" + 770892, "A" + 452179, "A" + 125 };
            PushAllItemsThenPopAllAndCheckOrder(data);
        }

        [Test]
        public void MixedPushPopSequence()
        {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(5);
            queue.Enqueue(3);

            Assert.AreEqual(5, queue.Dequeue());
            
            queue.Enqueue(6);
            Assert.AreEqual(6, queue.Dequeue());

            queue.Enqueue(4);
            Assert.AreEqual(4, queue.Dequeue());

            queue.Enqueue(14);
            Assert.AreEqual(14, queue.Dequeue());

            queue.Enqueue(1);
            Assert.AreEqual(3, queue.Dequeue());

            Assert.AreEqual(2, queue.Dequeue());
            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(1, queue.Dequeue());

            Assert.AreEqual(0, queue.Count);
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        private static void PushAllItemsThenPopAllAndCheckOrder<T>(T[] data)
        {
            var queue = new PriorityQueue<T>();
            Assert.AreEqual(0, queue.Count);

            for (int i = 0; i < data.Length; i++)
            {
                queue.Enqueue(data[i]);
                Assert.AreEqual(i + 1, queue.Count);
            }

            Array.Sort(data);

            for (int i = data.Length - 1; i >= 0; i--)
            {
                Assert.AreEqual(data[i], queue.Peek());
                Assert.AreEqual(data[i], queue.Dequeue());
                Assert.AreEqual(i, queue.Count);
            }

            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }
    }
}
