// <copyright file="IReversibleDictionary.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/01</date>

namespace Sinbadsoft.Lib.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary> Generic reversible dictionary. </summary>
    /// <typeparam name="TKey"> Type of the key elements. </typeparam>
    /// <typeparam name="TValue"> Type of the value elements. </typeparam>
    public interface IReversibleDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary> Gets the reverse dictionary. </summary>
        IReversibleDictionary<TValue, TKey> Reverse
        {
            get;
        }
    }

    /// <summary> Reversible dictionary. </summary>
    public interface IReversibleDictionary : IDictionary
    {
        /// <summary> Gets the reverse dictionary. </summary>
        IReversibleDictionary Reverse
        {
            get;
        }
    }
}