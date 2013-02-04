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
            queue.Push(1);
            queue.Push(2);
            queue.Push(5);
            queue.Push(3);

            Assert.AreEqual(5, queue.Pop());
            
            queue.Push(6);
            Assert.AreEqual(6, queue.Pop());

            queue.Push(4);
            Assert.AreEqual(4, queue.Pop());

            queue.Push(14);
            Assert.AreEqual(14, queue.Pop());

            queue.Push(1);
            Assert.AreEqual(3, queue.Pop());

            Assert.AreEqual(2, queue.Pop());
            Assert.AreEqual(1, queue.Pop());
            Assert.AreEqual(1, queue.Pop());

            Assert.AreEqual(0, queue.Size);
            Assert.Throws<InvalidOperationException>(() => queue.Top());
            Assert.Throws<InvalidOperationException>(() => queue.Pop());
        }

        private static void PushAllItemsThenPopAllAndCheckOrder<T>(T[] data)
        {
            var queue = new PriorityQueue<T>();
            Assert.AreEqual(0, queue.Size);

            for (int i = 0; i < data.Length; i++)
            {
                queue.Push(data[i]);
                Assert.AreEqual(i + 1, queue.Size);
            }

            Array.Sort(data);

            for (int i = data.Length - 1; i >= 0; i--)
            {
                Assert.AreEqual(data[i], queue.Top());
                Assert.AreEqual(data[i], queue.Pop());
                Assert.AreEqual(i, queue.Size);
            }

            Assert.Throws<InvalidOperationException>(() => queue.Pop());
        }
    }
}
