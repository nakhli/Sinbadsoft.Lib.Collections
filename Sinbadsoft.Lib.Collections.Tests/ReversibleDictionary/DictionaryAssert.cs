// <copyright file="DictionaryAssert.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/01</date>

namespace Sinbadsoft.Lib.Collections.Tests.ReversibleDictionary
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    /// <summary>
    /// Assertion class for <see cref="IDictionary{TKey,TValue}"/>
    /// and <see cref="IReversibleDictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryAssert
    {
        /// <summary>
        /// Checks 
        /// <see cref="IDictionary{TKey,TValue}.ContainsKey"/>,
        /// <see cref="ICollection{T}.Contains"/>,
        /// <see cref="IDictionary{TKey,TValue}.TryGetValue"/> and
        /// <see cref="IDictionary{TKey,TValue}.Item"/> getter are all 
        /// coherent and indicate that the given pair is in dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="dict"> The dictionary to check. </param>
        /// <param name="pair"> The pair expected to be found in the dictionary. </param>
        public static void Contains<TKey, TValue>(IDictionary<TKey, TValue> dict, KeyValuePair<TKey, TValue> pair)
        {
            Assert.IsTrue(dict.ContainsKey(pair.Key));
            Assert.IsTrue(dict.Contains(pair));
            TValue val;
            Assert.IsTrue(dict.TryGetValue(pair.Key, out val));
            Assert.AreEqual(val, pair.Value);
            Assert.AreEqual(dict[pair.Key], pair.Value);
        }

        /// <summary>
        /// Checks the given pair (key,value) is not in the dictionary.
        /// The given key or/and value may be in the dictionary but 
        /// they are not bound to each others.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="dict"> The dictionary to check. </param>
        /// <param name="pair"> The pair expected not to be found in the dictionary. </param>
        public static void DoesNotContain<TKey, TValue>(IDictionary<TKey, TValue> dict, KeyValuePair<TKey, TValue> pair)
        {
            TValue val;
            Assert.IsFalse(dict.TryGetValue(pair.Key, out val) && object.Equals(val, pair.Value));
        }

        /// <summary>
        /// Checks that
        /// <see cref="IDictionary{TKey,TValue}.ContainsKey"/>,
        /// <see cref="ICollection{T}.Contains"/> and
        /// <see cref="IDictionary{TKey,TValue}.TryGetValue"/>
        /// are all coherent and indicate that the given pair is in dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="dict"> The dictionary to check. </param>
        /// <param name="key"> The key expected not to be found in the dictionary. </param>
        public static void DoesNotContainKey<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key)
        {
            Assert.IsFalse(dict.ContainsKey(key));
            TValue val = default(TValue);
            Assert.IsFalse(dict.Contains(new KeyValuePair<TKey, TValue>(key, val)));
            Assert.IsFalse(dict.TryGetValue(key, out val));
        }

        /// <summary>
        /// Expects the dictionary to throw an <see cref="ArgumentException"/>  exception on 
        /// <see cref="ICollection{T}.Add"/> and <see cref="IDictionary{TKey,TValue}.Add(TKey,TValue)"/>.
        /// This method launches and catches exceptions and might slow execution if used heavily.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam> 
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="dict"> The dictionary to check. </param>
        /// <param name="pair"> The pair expected to be in the dictionary. </param>
        public static void ThrowsOnAdd<TKey, TValue>(IDictionary<TKey, TValue> dict, KeyValuePair<TKey, TValue> pair)
        {
            Assert.Throws<ArgumentException>(
                () => dict.Add(pair.Key, pair.Value), 
                "On key '{0}' and value '{1}').", 
                pair.Key, 
                pair.Value);

            Assert.Throws<ArgumentException>(
                () => dict.Add(pair),
                "On pair ('{0}', '{1}').",
                pair.Key,
                pair.Value);
        }

        /// <summary> Expects the dictionary to throw a <see cref="KeyNotFoundException"/>  exception on 
        /// <see cref="IDictionary{TKey,TValue}.Item"/> indexer get.
        /// This method launches and catches exceptions and might slow execution if used heavily.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="dict"> The dictionary to check. </param>
        /// <param name="key"> The key expected to be not in the dictionary. </param>
        public static void ThrowsOnGetItem<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key)
        {
            Assert.Throws<KeyNotFoundException>(() => { TValue v = dict[key]; }, "On key '{0}'.", key);
        }
    }
}