// <copyright file="DynamicArrayDebugView.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/15</date>

namespace Sinbadsoft.Lib.Collections
{
    using System;
    using System.Diagnostics;

    internal class DynamicArrayDebugView<T>
    {
        private readonly Array view;

        public DynamicArrayDebugView(DynamicArray<T> dynarray)
        {
            this.view = dynarray.ToArray();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Array View
        {
            get
            {
                return this.view;
            }
        }
    }
}