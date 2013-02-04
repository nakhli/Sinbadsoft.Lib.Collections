// <copyright file="ResizeInsertTests.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/08</date>

namespace Sinbadsoft.Lib.Collections.Tests.DynamicArray
{
    using System;
    using System.Collections;
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class ResizeInsertTests
    {
        private static readonly int[] Zero2DIdx = { 0, 0 };

        [Test]
        public void ExceptionOnInsertArrayOfInvalidRank()
        {
            DynamicArray<int> dynArray = new DynamicArray<int>(2);
            Assert.Throws<RankException>(() => dynArray.Insert(new int[5], 0, 0, 0));
            Assert.Throws<RankException>(() => dynArray.Insert(new int[5, 10, 12], 0, 0, 0));
        }

        [Test]
        public void ExceptionOnInsertArrayWithANullLength()
        {
            var dynArray = new DynamicArray<int>(2);
            Assert.Throws<ArgumentException>(() => dynArray.Insert(new int[5, 0], 0, 0, 0));
        }

        [Test]
        public void foobar()
        {
            var array = new DynamicArray<string>(2);
            array.Insert(new[,] { { "0,0", "0,1" }, { "1,0", "1,1" } }, 0, 0, 0);
            array.ResizeDim(0, 1);
            array.Resize(0, 0);
            Console.WriteLine(array.ToString());
        }

        [Test]
        public void ResizeWithSameSize()
        {
            var darray = new DynamicArray<int>(1);
            
            IEnumerator invalidated = darray.GetEnumerator();
            Assert.AreEqual(0, darray.Count);
            darray.Resize(100);
            Assert.AreEqual(100, darray.Count);
            Assert.Throws<InvalidOperationException>(() => invalidated.MoveNext());
            
            IEnumerator notInvalidated = darray.GetEnumerator();
            Assert.AreEqual(100, darray.Count);
            darray.Resize(100);
            Assert.AreEqual(100, darray.Count);
            Assert.DoesNotThrow(() => notInvalidated.MoveNext());
        }

        [Test]
        public static void Insert2D()
        {
            const int O_ROWS = 4;
            const int O_COLS = 2;
            const int A_ROWS = 4;
            const int A_COLS = 3;
            const int B_ROWS = 2;
            const int B_COLS = 2;

            // Arrays O, A and B
            object[,] oarray = ArrayHelper.New().NewArray<object>(O_ROWS, O_COLS).FillWith("O").As<object[,]>();
            object[,] aarray = ArrayHelper.New().NewArray<object>(A_ROWS, A_COLS).FillWith("A").As<object[,]>();
            object[,] barray = ArrayHelper.New().NewArray<object>(B_ROWS, B_COLS).FillWith("B").As<object[,]>();

            // NOTE capacities are set to the minimum to force buffer resize 
            DynamicArray<object> dynArray = new DynamicArray<object>(2, 1, 1);

            // Insert O at (O_ROW_POS, O_COL_POS)
            const int O_ROW_POS = 0;
            const int O_COL_POS = 2;
            dynArray.Insert(oarray, 0, O_ROW_POS, O_COL_POS);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, oarray, O_ROW_POS, O_COL_POS);
            DynamicArrayAssert.CountsEqual(dynArray, O_ROW_POS + O_ROWS, O_COL_POS + O_COLS);

            // Insert A at (A_ROW_POS, A_COL_POS) in dim 0
            // Must have: (A_ROW_POS > O_ROW_POS) && (A_ROW_POS < O_ROW_POS + O_ROWS) 
            // in order to insert A "within" O
            const int A_ROW_POS = 1;
            const int A_COL_POS = 1;
            Assert.IsTrue(A_ROW_POS > O_ROW_POS);
            Assert.IsTrue(A_ROW_POS < O_ROW_POS + O_ROWS);
            dynArray.Insert(aarray, 0, A_ROW_POS, A_COL_POS);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, aarray, A_ROW_POS, A_COL_POS);
            DynamicArrayAssert.Included(
                    dynArray,
                    oarray,
                    new int[] { O_ROW_POS, O_COL_POS },
                    Zero2DIdx,
                    new int[] { A_ROW_POS - O_ROW_POS, O_COLS });
            DynamicArrayAssert.Included(
                    dynArray,
                    oarray,
                    new int[] { A_ROW_POS + A_ROWS, O_COL_POS },
                    new int[] { A_ROW_POS - O_ROW_POS, 0 },
                    new int[] { O_ROWS, O_COLS });

            // Insert B at (B_ROW_POS, B_COL_POS) in dim 1
            const int B_ROW_POS = 2;
            const int B_COL_POS = 1;
            dynArray.Insert(barray, 1, B_ROW_POS, B_COL_POS);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, barray, B_ROW_POS, B_COL_POS);
            DynamicArrayAssert.Included(
                    dynArray,
                    aarray,
                    new int[] { B_ROW_POS, B_COL_POS + B_COLS },
                    new int[] { B_ROW_POS - A_ROW_POS, B_COL_POS - A_COL_POS },
                    new int[] { B_ROW_POS - A_ROW_POS + B_ROWS, A_COLS });
        }

        [Test]
        public static void Resize2D()
        {
            // Should have B_COLS < A_COLS
            const int A_ROWS = 4;
            const int A_COLS = 3;
            const int B_ROWS = 5;
            const int B_COLS = 2;

            // Arrays O, A and B
            object[,] arrA = ArrayHelper.New().NewArray<object>(A_ROWS, A_COLS).FillWith("A").As<object[,]>();
            object[,] arrB = ArrayHelper.New().NewArray<object>(B_ROWS, B_COLS).FillWith("B").As<object[,]>();

            // NOTE capacities are set to the minimum to force buffer resize 
            DynamicArray<object> dynArray = new DynamicArray<object>(2, 1, 1);

            // Insert B at 0,0
            dynArray.Insert(arrB, 0, 0, 0);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.AreElementsEqual(dynArray, arrB);

            // Insert A right before B in dim 1
            dynArray.Insert(arrA, 0, 0, 0);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, arrA, 0, 0);
            DynamicArrayAssert.Included(dynArray, arrB, A_ROWS, 0);
            DynamicArrayAssert.CountsEqual(dynArray, A_ROWS + B_ROWS, A_COLS);

            // Remove B_ROWS lines and add NCols columns
            const int NCols = 5;
            dynArray.Resize(A_ROWS, A_COLS + NCols);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, arrA, 0, 0);
            DynamicArrayAssert.AreElementsDefault(dynArray, new int[] { A_ROWS, NCols }, 0, A_COLS);
            DynamicArrayAssert.CountsEqual(dynArray, A_ROWS, A_COLS + NCols);

            // Add NRows rows
            const int NRows = 10;
            dynArray.ResizeDim(0, A_ROWS + NRows);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.CountsEqual(dynArray, A_ROWS + NRows, A_COLS + NCols);
            DynamicArrayAssert.Included(dynArray, arrA, 0, 0);
            DynamicArrayAssert.AreElementsDefault(dynArray, new int[] { NRows, A_COLS + NCols }, A_ROWS, 0);
            DynamicArrayAssert.AreElementsDefault(dynArray, new int[] { A_ROWS + NRows, NCols }, 0, A_COLS);

            // Shrink to (NFewRows, NFewCols)
            const int NFewCols = 2;
            const int NFewRows = 3;
            dynArray.Resize(NFewRows, NFewCols);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, arrA, Zero2DIdx, Zero2DIdx, new int[] { NFewRows, NFewCols });

            // Enlarge to (NManyRows, NManyCols) 
            const int NManyRows = 10;
            const int NManyCols = 7;
            dynArray.Resize(NManyRows, NManyCols);
            ArrayHelper.Print(dynArray);
            DynamicArrayAssert.Included(dynArray, arrA, Zero2DIdx, Zero2DIdx, new int[] { NFewRows, NFewCols });
            DynamicArrayAssert.AreElementsDefault(dynArray, new[] { NManyRows, NManyCols - NFewCols }, 0, NFewCols);
            DynamicArrayAssert.AreElementsDefault(dynArray, new[] { NManyRows - NFewRows, NFewCols }, NFewRows, 0);
        }

        [Test, Sequential]
        public void InsertTest(
            [Values(2, 987, 100, 17, 3, 2, 1)]int length,
            [Values(1, 2, 2, 3, 3, 2, 16)] int rank,
            [Values(0, 0, 1, 2, 2, 1, 5)] int dim)
        {
            var idata = new InsertionData(length, rank, dim);
            var zeroIdx = IndexesHelper.Zero(idata.Rank);

            // Build O and A hypercubes lengths
            var alengths = IndexesHelper.Add(zeroIdx, idata.Length);
            var olengths = IndexesHelper.Multiply(alengths, 2);

            // Build O and A hypercubes
            var oarray = ArrayHelper.New().NewArray<object>(olengths).FillWith("O").As<Array>();
            var aarray = ArrayHelper.New().NewArray<object>(alengths).FillWith("A").As<Array>();

            // NOTE capacities are set to the minimum to force buffer resize
            var capacities = IndexesHelper.Add(IndexesHelper.Zero(idata.Rank), 1);
            var dynarray = new DynamicArray<string>(idata.Rank, capacities);

            dynarray.Insert(oarray, 0, zeroIdx);
            DynamicArrayAssert.Included(dynarray, oarray, IndexesHelper.Zero(idata.Rank));
            DynamicArrayAssert.CountsEqual(dynarray, olengths);

            var pos = IndexesHelper.Add(alengths, -1);
            var expected = IndexesHelper.Clone(olengths);
            expected[idata.Dim] += alengths[idata.Dim];
            dynarray.Insert(aarray, idata.Dim, pos);
            DynamicArrayAssert.CountsEqual(dynarray, expected);
            DynamicArrayAssert.Included(dynarray, aarray, pos);
            DynamicArrayAssert.Included(
                dynarray, 
                oarray,
                IndexesHelper.Add(pos, alengths),
                idata.Rank > 1 ? IndexesHelper.Add(pos, alengths) : pos, 
                olengths);
        }

        private struct InsertionData
        {
            /// <summary> Hypercube length. </summary>
            public readonly int Length;

            /// <summary> Hypercube rank. </summary>
            public readonly int Rank;

            /// <summary> Dimension where to insert. </summary>
            public readonly int Dim;

            public InsertionData(int length, int rank, int dim)
            {
                this.Length = length;
                this.Rank = rank;
                this.Dim = dim;
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.InvariantCulture, "Rank ({0}) Length:{1} Dim:{2}", this.Rank, this.Length, this.Dim);
            }
        }
    }
}