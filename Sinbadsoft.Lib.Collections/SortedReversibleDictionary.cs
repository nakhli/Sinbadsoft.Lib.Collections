// <copyright file="SortedReversibleDictionary.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/04/17</date>

namespace Sinbadsoft.Lib.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary> 
    /// Implementation of IReversibleDictionary based on <see cref="SortedDictionary{TKey,TValue}"/>. 
    /// </summary>
    /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
    /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class SortedReversibleDictionary<TKey, TValue> : 
        AbstractReversibleDictionary<TKey, TValue, SortedDictionary<TKey, TValue>, SortedDictionary<TValue, TKey>>,
        IReversibleDictionary<TKey, TValue>,
        IReversibleDictionary,
        ISerializable,
        IDeserializationCallback
    {
        private const string ValueComparerName = "ValueComparer";
        private const string KeyComparerName = "KeyComparer";
        private SortedReversibleDictionary<TValue, TKey> reversed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedReversibleDictionary{TKey,TValue}"/> class.
        /// </summary>
        public SortedReversibleDictionary()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedReversibleDictionary{TKey,TValue}"/> class.
        /// The <see cref="SortedReversibleDictionary{TKey,TValue}"/> will be initialized with the key and value pairs
        /// contained in <paramref name="data"/>.
        /// </summary>
        /// <param name="data"> Contains the key and value pairs for the 
        /// <see cref="SortedReversibleDictionary{TKey,TValue}"/> intialization. </param>
        public SortedReversibleDictionary(IEnumerable<KeyValuePair<TKey, TValue>> data)
            : this(null, null)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            AddAll(data);
        }

        public SortedReversibleDictionary(
            IEnumerable<KeyValuePair<TKey, TValue>> data,
            IComparer<TKey> keyComparer, 
            IComparer<TValue> valueComparer)
            : this(keyComparer, valueComparer)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            AddAll(data);
        }

        public SortedReversibleDictionary(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
        {
            this.Key2Value = new SortedDictionary<TKey, TValue>(keyComparer);
            this.Value2Key = new SortedDictionary<TValue, TKey>(valueComparer);
            this.reversed = new SortedReversibleDictionary<TValue, TKey>(this.Value2Key, this.Key2Value, this);
        }

        protected SortedReversibleDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private SortedReversibleDictionary(
           SortedDictionary<TKey, TValue> key2value,
           SortedDictionary<TValue, TKey> value2key,
           SortedReversibleDictionary<TValue, TKey> reverse)
            : base(key2value, value2key)
        {
            this.reversed = reverse;
        }

        public IComparer<TKey> KeyComparer
        {
            get { return this.Key2Value.Comparer; }
        }

        public IComparer<TValue> ValueComparer
        {
            get { return this.Value2Key.Comparer; }
        }

        /// <summary> Gets the reverse dictionary. </summary>
        public SortedReversibleDictionary<TValue, TKey> Reverse
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
            info.AddValue(KeyComparerName, this.Key2Value.Comparer, typeof(IComparer<TKey>));
            info.AddValue(ValueComparerName, this.Value2Key.Comparer, typeof(IComparer<TKey>));
        }
        
        public void OnDeserialization(object sender)
        {
            if (this.SInfo == null)
            {
                return;
            }

            KeyValuePair<TKey, TValue>[] serializedKey2Value = this.DeserializeElements();

            IComparer<TKey> serializedKey2ValueComparer;
            this.Key2Value = TryDeserializeValue(KeyComparerName, out serializedKey2ValueComparer, sender) 
                ? new SortedDictionary<TKey, TValue>(serializedKey2ValueComparer)
                : new SortedDictionary<TKey, TValue>();

            IComparer<TValue> serializedValue2KeyComparer;
            this.Value2Key = TryDeserializeValue(ValueComparerName, out serializedValue2KeyComparer, sender)
                ? new SortedDictionary<TValue, TKey>(serializedValue2KeyComparer)
                : new SortedDictionary<TValue, TKey>();

            this.reversed = new SortedReversibleDictionary<TValue, TKey>(this.Value2Key, this.Key2Value, this);
            this.AddAll(serializedKey2Value);
            this.SInfo = null;
        }
    }
}
