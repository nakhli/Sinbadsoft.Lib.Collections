// <copyright file="ReversibleDictionary.cs" company="Sinbadsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary> 
    /// Implementation of IReversibleDictionary based on <see cref="Dictionary{TKey,TValue}"/>. 
    /// </summary>
    /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
    /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// Retrieving a value by using its key is very fast, close to O(1), as this implementation is based on a 
    /// <see cref="Dictionary{TKey,TValue}"/> which is implemented as a hash table.
    /// </item>
    /// <item>
    /// As for <see cref="Dictionary{TKey,TValue}"/>, The speed of retrieval depends on the quality of the hashing 
    /// algorithm of the types <em>TKey</em> and <em>TValue</em>.
    /// </item>
    /// <item>
    /// As long as an object is used as a key ot value in the <see cref="ReversibleDictionary{TKey,TValue}"/>, 
    /// it must not change in any way that affects its hash value. Every key and value in a <see cref="ReversibleDictionary{TKey,TValue}"/>
    /// must be unique according to the dictionary's equality comparers. A key or a value cannot be <see langword="null"/>.
    /// </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class ReversibleDictionary<TKey, TValue> :
        AbstractReversibleDictionary<TKey, TValue, Dictionary<TKey, TValue>, Dictionary<TValue, TKey>>,
        IReversibleDictionary<TKey, TValue>, 
        IReversibleDictionary,
        IDeserializationCallback
    {
        private const string ValueEqualityComparerName = "ValueEqualityComparer";
        private const string KeyEqualityComparerName = "KeyEqualityComparer";
        private ReversibleDictionary<TValue, TKey> reversed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReversibleDictionary{TKey,TValue}"/> class.
        /// </summary>
        public ReversibleDictionary()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReversibleDictionary{TKey,TValue}"/> class.
        /// The <see cref="ReversibleDictionary{TKey,TValue}"/> will be initialized with the key and value pairs 
        /// contained in <paramref name="data"/>.
        /// </summary>
        /// <param name="data"> Contains the key and value pairs for the 
        /// <see cref="ReversibleDictionary{TKey,TValue}"/> intialization. </param>
        public ReversibleDictionary(IEnumerable<KeyValuePair<TKey, TValue>> data)
            : this(null, null)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.AddAll(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReversibleDictionary{TKey,TValue}"/> class that
        /// uses the provided <see cref="IEqualityComparer{T}"/> for <typeparamref name="TKey"/> and 
        /// <typeparamref name="TValue"/>. The <see cref="ReversibleDictionary{TKey,TValue}"/> will be initialized
        /// with the key and value pairs contained in <paramref name="data"/>.
        /// </summary>
        /// <param name="data"> Contains the key and value pairs for the 
        /// <see cref="ReversibleDictionary{TKey,TValue}"/> intialization. </param>
        /// <param name="keyComparer"> The <see cref="IEqualityComparer{T}"/> implementation to use when comparing 
        /// keys, or <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> for the type
        /// of the key.
        /// </param>
        /// <param name="valueComparer"> The <see cref="IEqualityComparer{T}"/> implementation to use when comparing 
        /// values, or <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> for the type
        /// of the values.</param>
        public ReversibleDictionary(
            IEnumerable<KeyValuePair<TKey, TValue>> data,
            IEqualityComparer<TKey> keyComparer, 
            IEqualityComparer<TValue> valueComparer)
            : this(keyComparer, valueComparer)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.AddAll(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReversibleDictionary{TKey,TValue}"/> class that
        /// uses the provided <see cref="IEqualityComparer{T}"/> for <typeparamref name="TKey"/> and 
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="keyComparer"> The <see cref="IEqualityComparer{T}"/> implementation to use when comparing 
        /// keys, or <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> for the type
        /// of the key.
        /// </param>
        /// <param name="valueComparer"> The <see cref="IEqualityComparer{T}"/> implementation to use when comparing 
        /// values, or <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> for the type
        /// of the values.</param>
        public ReversibleDictionary(
            IEqualityComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueComparer)
        {
            this.Key2Value = new Dictionary<TKey, TValue>(keyComparer);
            this.Value2Key = new Dictionary<TValue, TKey>(valueComparer);
            this.reversed = new ReversibleDictionary<TValue, TKey>(this.Value2Key, this.Key2Value, this);
        }

        protected ReversibleDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private ReversibleDictionary(
            Dictionary<TKey, TValue> key2Value, 
            Dictionary<TValue, TKey> value2Key,
            ReversibleDictionary<TValue, TKey> reverse) 
            : base(key2Value, value2Key)
        {
            this.reversed = reverse;
        }

        public IEqualityComparer<TKey> KeyComparer
        {
            get { return this.Key2Value.Comparer; }
        }

        public IEqualityComparer<TValue> ValueComparer
        {
            get { return this.Value2Key.Comparer; }
        }

        /// <summary> Gets the reverse dictionary. </summary>
        public ReversibleDictionary<TValue, TKey> Reverse
        {
            get { return this.reversed; }
        }

        /// <summary> Gets the reverse dictionary. </summary>
        IReversibleDictionary IReversibleDictionary.Reverse
        {
            get { return this.reversed; }
        }

        /// <summary> Gets the reverse dictionary. </summary>
        IReversibleDictionary<TValue, TKey> IReversibleDictionary<TKey, TValue>.Reverse
        {
            get { return this.reversed; }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(KeyEqualityComparerName, this.Key2Value.Comparer, typeof(IEqualityComparer<TKey>));
            info.AddValue(ValueEqualityComparerName, this.Value2Key.Comparer, typeof(IEqualityComparer<TKey>));
        }
        
        public void OnDeserialization(object sender)
        {
            if (this.SInfo == null)
            {
                return;
            }

            KeyValuePair<TKey, TValue>[] serializedKey2Value = this.DeserializeElements();

            IEqualityComparer<TKey> serializedKey2ValueComparer;
            IEqualityComparer<TValue> serializedValue2KeyComparer;

            this.Key2Value = this.TryDeserializeValue(KeyEqualityComparerName, out serializedKey2ValueComparer, sender)
                ? new Dictionary<TKey, TValue>(serializedKey2ValueComparer)
                : new Dictionary<TKey, TValue>();

            this.Value2Key = this.TryDeserializeValue(ValueEqualityComparerName, out serializedValue2KeyComparer, sender)
                 ? new Dictionary<TValue, TKey>(serializedValue2KeyComparer)
                 : new Dictionary<TValue, TKey>();
            
            this.reversed = new ReversibleDictionary<TValue, TKey>(this.Value2Key, this.Key2Value, this);
            this.AddAll(serializedKey2Value);
            this.SInfo = null;
        }
    }
}