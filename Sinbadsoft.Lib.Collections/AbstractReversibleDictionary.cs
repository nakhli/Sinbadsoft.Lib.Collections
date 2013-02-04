// <copyright file="AbstractReversibleDictionary.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/04/15</date>

namespace Sinbadsoft.Lib.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Base abstract class for reversible dictionaries. This base implementation uses
    /// two dictionaries for key->value and value->key maps.
    /// </summary>
    /// <typeparam name="TKey"> The type of the keys in the dictionary. </typeparam>
    /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
    /// <typeparam name="TDictImpl"> The implementation type of 
    /// <c>(<typeparamref name="TKey"/>, <typeparamref name="TValue"/>)</c> map.</typeparam>
    /// <typeparam name="TDictImplRev"> The implementation type of 
    /// <c>(<typeparamref name="TValue"/>, <typeparamref name="TKey"/>)</c> map.</typeparam>
    [Serializable]
    public abstract class AbstractReversibleDictionary<TKey, TValue, TDictImpl, TDictImplRev> :
        IDictionary<TKey, TValue>, IDictionary, ISerializable
        where TDictImpl : IDictionary<TKey, TValue>, IDictionary, new()
        where TDictImplRev : IDictionary<TValue, TKey>, IDictionary, new()
    {
        protected const string Key2ValueName = "Key2Value";
        private SerializationInfo sinfo;
        private TDictImpl key2value;
        private TDictImplRev value2key;

        protected AbstractReversibleDictionary()
        {
            this.key2value = new TDictImpl();
            this.value2key = new TDictImplRev();
        }

        protected AbstractReversibleDictionary(
                TDictImpl keyToValue,
                TDictImplRev valueToKey)
        {
            this.key2value = keyToValue;
            this.value2key = valueToKey;
        }

        protected AbstractReversibleDictionary(SerializationInfo info, StreamingContext context)
        {
            // NOTE the actual deserialization work will be done in OnDeserialization
            this.sinfo = info;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}"/>.
        /// </returns>
        public int Count
        {
            get { return this.Key2ValueAsGenericDictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IDictionary"/> object has a fixed size.
        /// </summary>
        /// <returns>
        /// true if the <see cref="IDictionary"/> object has a fixed size; otherwise, false.
        /// </returns>
        public bool IsFixedSize
        {
            get { return this.key2value.IsFixedSize; }
        }

        /// <summary> Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only. </summary>
        /// <returns> <see langword="true"/> if the <see cref="ICollection{T}"/> is read-only; otherwise, <see langword="false"/>. </returns>
        public bool IsReadOnly
        {
            get { return this.Key2ValueAsGenericCollection.IsReadOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if access to the <see cref="ICollection"/> is synchronized (thread safe); 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSynchronized
        {
            get { return this.key2value.IsSynchronized; }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the keys of the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{T}"/> containing the keys of the object that 
        /// implements <see cref="IDictionary{TKey,TValue}"/>.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get { return this.Key2ValueAsGenericDictionary.Keys; }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the values in the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{T}"/> containing the values in the object that implements <see cref="IDictionary{TKey,TValue}"/>.
        /// </returns>
        public ICollection<TValue> Values
        {
            get { return this.Key2ValueAsGenericDictionary.Values; }
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> object containing the values in the <see cref="IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection"/> object containing the values in the <see cref="IDictionary"/> object.
        /// </returns>
        ICollection IDictionary.Values
        {
            get { return this.Key2ValueAsDictionary.Values; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>. 
        /// </summary>
        /// <returns> An object that can be used to synchronize access to the <see cref="ICollection"/>. </returns>
        public object SyncRoot
        {
            // NOTE must ensure SyncRoot==Reverse.SyncRoot
            get { return this; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection"/>.
        /// </returns>
        int ICollection.Count
        {
            get { return this.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IDictionary"/> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="IDictionary"/> object is read-only; otherwise, false.
        /// </returns>
        bool IDictionary.IsReadOnly
        {
            get { return this.Key2ValueAsDictionary.IsReadOnly; }
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> object containing the keys of the <see cref="IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection"/> object containing the keys of the <see cref="IDictionary"/> object.
        /// </returns>
        ICollection IDictionary.Keys
        {
            get
            {
                return this.Key2ValueAsDictionary.Keys;
            }
        }

        protected TDictImpl Key2Value
        {
            get { return this.key2value; }
            set { this.key2value = value; }
        }

        protected TDictImplRev Value2Key
        {
            get { return this.value2key; }
            set { this.value2key = value; }
        }

        protected SerializationInfo SInfo
        {
            get { return this.sinfo; }
            set { this.sinfo = value; }
        }

        private IDictionary<TKey, TValue> Key2ValueAsGenericDictionary
        {
            get { return this.key2value; }
        }

        private IDictionary Key2ValueAsDictionary
        {
            get { return this.key2value; }
        }

        private ICollection<KeyValuePair<TKey, TValue>> Key2ValueAsGenericCollection
        {
            get { return this.key2value; }
        }

        private ICollection Key2ValueAsCollection
        {
            get { return this.key2value; }
        }

        /// <summary> Gets or sets the element with the specified key. </summary>
        /// <returns> The element with the specified key. </returns>
        /// <param name="key">The key of the element to get or set.</param>
        /// <exception cref="NotSupportedException">The property is set and the <see cref="IDictionary{TKey,TValue}"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException">key is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and key is not found.</exception>
        public TValue this[TKey key]
        {
            get 
            { 
                return this.key2value[key]; 
            }

            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                TValue oldValue; // (key, oldValue) if any in this._key2value
                TKey oldKey; // (value, oldKey) if any in this._value2key

                bool keyAlreadyInKey2Value = this.key2value.TryGetValue(key, out oldValue);
                bool valueAlreadyInValue2Key = this.value2key.TryGetValue(value, out oldKey);

                if (keyAlreadyInKey2Value)
                {
                    this.key2value.Remove(key);
                    this.value2key.Remove(oldValue);
                }

                if (valueAlreadyInValue2Key)
                {
                    this.key2value.Remove(oldKey);
                    this.value2key.Remove(value);
                }

                this.key2value[key] = value;
                this.value2key[value] = key;
            }
        }

        /// <summary> Gets or sets the element with the specified key. </summary>
        /// <returns> The element with the specified key. </returns>
        /// <param name="key">The key of the element to get or set. </param>
        /// <exception cref="NotSupportedException">The property is set and the <see cref="IDictionary"/> object is read-only.-or- The property is set, key does not exist in the collection, and the <see cref="IDictionary"/> has a fixed size. </exception>
        /// <exception cref="ArgumentNullException">key is <see langword="null"/>. </exception>
        object IDictionary.this[object key]
        {
            get
            {
                VerifyKeyType(key);
                return this[(TKey)key];
            }

            set
            {
                VerifyKeyType(key);
                VerifyValueType(value);
                this[(TKey)key] = (TValue)value;
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key"> The object to use as the key of the element to add.</param>
        /// <param name="value"> The object to use as the value of the element to add.</param>
        /// <exception cref="NotSupportedException">The <see cref="IDictionary{TKey,TValue}"/> is read-only.
        /// </exception>
        /// <exception cref="ArgumentException">An element with the same key already exists in the 
        /// <see cref="IDictionary{TKey,TValue}"/>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="key"/> is <see langword="null"/>.</exception>
        public void Add(TKey key, TValue value)
        {
            this.key2value.Add(key, value);
            try
            {
                this.value2key.Add(value, key);
            }
            catch
            {
                this.key2value.Remove(key);
                throw;
            }
        }

        /// <summary> Adds an item to the <see cref="ICollection{T}"/>. </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>. </param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only. </exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only. </exception>
        public void Clear()
        {
            this.Key2ValueAsGenericCollection.Clear();
            ((ICollection<KeyValuePair<TValue, TKey>>)this.value2key).Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <returns> Returns <see langword="true"/> if <paramref name="item"/> is found in the 
        /// <see cref="ICollection{T}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="item"> The object to locate in the <see cref="ICollection{T}"/>.</param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.key2value.Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="IDictionary{TKey,TValue}"/> contains an element with the specified key.
        /// </summary>
        /// <returns> Returns <see langword="true"/> if the <see cref="IDictionary{TKey,TValue}"/> contains an 
        /// element with the provided key; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="key"> The key to locate in the <see cref="IDictionary{TKey,TValue}"/>.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        public bool ContainsKey(TKey key)
        {
            return this.key2value.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>, 
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> 
        /// that is the destination of the elements copied from <see cref="ICollection{T}"/>. 
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is multidimensional -or- arrayIndex is equal 
        /// to or greater than the length of array.-or-The number of elements in the source 
        /// <see cref="ICollection{T}"/> is greater than the available space from arrayIndex to the end 
        /// of the destination array.-or-Type T cannot be cast automatically to the type of the destination 
        /// array.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.key2value.CopyTo(array, arrayIndex);
        }

        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.Key2ValueAsGenericDictionary.GetEnumerator();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if the element is successfully removed; otherwise, <see langword="false"/>. 
        /// This method also returns <see langword="false"/> if key was not found in the original 
        /// <see cref="IDictionary{TKey,TValue}"/>.
        /// </returns>
        /// <param name="key"> The key of the element to remove.</param>
        /// <exception cref="NotSupportedException"> The <see cref="IDictionary{TKey,TValue}"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException"> The key is <see langword="null"/>.</exception>
        public bool Remove(TKey key)
        {
            TValue val;
            if (this.key2value.TryGetValue(key, out val))
            {
                this.key2value.Remove(key);
                this.value2key.Remove(val);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns> Returns <see langword="true"/> if item was successfully removed from the 
        /// <see cref="ICollection{T}"/>; otherwise, <see langword="false"/>. This method also returns 
        /// <see langword="false"/> if item is not found in the original <see cref="ICollection{T}"/>.
        /// </returns>
        /// <param name="item"> The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException"> The <see cref="ICollection{T}"/> is read-only.</exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <summary>
        /// Attempts to get the element associated with a key.
        /// </summary>
        /// <param name="key"> The key of the element to look for. </param>
        /// <param name="value">Will be set to the element with key <paramref name="key"/> if any; otherwise it will
        /// be set to <see langword="default"/>(<typeparamref name="TKey"/>).</param>
        /// <returns> Returns <see langword="true"/> if an element with key <paramref name="key"/> is found;
        /// otherwise <see langword="false"/>. </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.key2value.TryGetValue(key, out value);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            IDictionary<TKey, TValue> dict = this.Key2ValueAsGenericDictionary;
            KeyValuePair<TKey, TValue>[] data = new KeyValuePair<TKey, TValue>[dict.Count];
            dict.CopyTo(data, 0);
            info.AddValue(Key2ValueName, data, typeof(KeyValuePair<TKey, TValue>[]));
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IDictionary"/> object.
        /// </summary>
        /// <param name="key">The <see cref="Object"></see> to use as the key of the element to add. </param>
        /// <param name="value">The <see cref="Object"></see> to use as the value of the element to add. </param>
        /// <exception cref="ArgumentException">An element with the same key already exists in the 
        /// <see cref="IDictionary"/> object. </exception>
        /// <exception cref="ArgumentNullException">key is <see langword="null"/>. </exception>
        /// <exception cref="NotSupportedException">The <see cref="IDictionary"/> is read-only has a fixed size.
        /// </exception>
        void IDictionary.Add(object key, object value)
        {
            VerifyKeyType(key);
            VerifyValueType(value);
            this.Add((TKey)key, (TValue)value);
        }

        /// <summary>
        /// Removes all elements from the <see cref="IDictionary"/> object.
        /// </summary>
        /// <exception cref="NotSupportedException">The <see cref="IDictionary"/> object is read-only. </exception>
        void IDictionary.Clear()
        {
            this.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="IDictionary"/> object contains an element with the specified key.
        /// </summary>
        /// <returns> Returns <see langword="true"/> if the <see cref="IDictionary"/> contains an element with the 
        /// provided key; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="IDictionary"/> object.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="key"/> is <see langword="null"/>. 
        /// </exception>
        bool IDictionary.Contains(object key)
        {
            return this.Key2ValueAsDictionary.Contains(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection"/> to an <see cref="Array"></see>, starting at a 
        /// particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="arr">The one-dimensional <see cref="Array"></see> that is the destination of the elements 
        /// copied from <see cref="ICollection"/>. The <see cref="Array"></see> must have zero-based indexing. 
        /// </param>
        /// <param name="index">The zero-based index in array at which copying begins. </param>
        /// <exception cref="ArgumentNullException">If <paramref name="arr"/> is <see langword="null"/>. </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="index"/> is less than zero. </exception>
        /// <exception cref="ArgumentException">array is multidimensional.-or- index is equal to or greater than the
        /// length of array.-or- The number of elements in the source <see cref="ICollection"/> is greater than the 
        /// available space from index to the end of the destination array. </exception>
        /// <exception cref="InvalidCastException">The type of the source <see cref="ICollection"/> cannot be cast 
        /// automatically to the type of the destination array. </exception>
        void ICollection.CopyTo(Array arr, int index)
        {
            this.Key2ValueAsCollection.CopyTo(arr, index);
        }

        /// <summary>
        /// Returns an <see cref="IDictionaryEnumerator"/> object for the <see cref="IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="IDictionaryEnumerator"/> object for the <see cref="IDictionary"/> object.
        /// </returns>
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.Key2ValueAsDictionary.GetEnumerator();
        }

        /// <summary> Returns an enumerator that iterates through a collection. </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Key2ValueAsCollection.GetEnumerator();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="IDictionary"/> object.
        /// </summary>
        /// <param name="key">The key of the element to remove. </param>
        /// <exception cref="NotSupportedException">The <see cref="IDictionary"/> object is read-only or 
        /// has a fixed size. </exception>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="key"/> is <see langword="null"/>.
        /// </exception>
        void IDictionary.Remove(object key)
        {
            if (key is TKey)
            {
                this.Remove((TKey)key);
            }
        }

        /// <summary>
        /// Invokes deserialization callback <see cref="IDeserializationCallback.OnDeserialization"/>
        /// for objects implementing the <see cref="IDeserializationCallback"/> interface.
        /// </summary>
        /// <param name="elt"> The object being deserilazed. </param>
        /// <param name="sender"> The sender to use as argument for deserialisation callback. </param>
        protected static void InvokeDeserializationCallback(object elt, object sender)
        {
            var toBeNotified = elt as IDeserializationCallback;
            if (toBeNotified != null)
            {
                toBeNotified.OnDeserialization(sender);
            }
        }

        protected KeyValuePair<TKey, TValue>[] DeserializeElements()
        {
            return (KeyValuePair<TKey, TValue>[])
                this.sinfo.GetValue(Key2ValueName, typeof(KeyValuePair<TKey, TValue>[]));
        }

        /// <summary>
        /// Extracts the element with the provided name form <see cref="SInfo"/>.
        /// </summary>
        /// <typeparam name="T"> The type of the element to extract from <see cref="SInfo"/>. </typeparam>
        /// <param name="name"> The name of the element to extract form <see cref="SInfo"/>. </param>
        /// <param name="elt"> Will be set to the element with name <paramref name="name"/> in <see cref="SInfo"/>
        /// if found; otherwise it will be set to <see langword="default"/>(<typeparamref name="T"/>). </param>
        /// <param name="sender"> The sender object to forward to 
        /// <see cref="IDeserializationCallback.OnDeserialization"/>. </param>
        /// <returns> Returns <see langword="true"/> if the element is found; otherwise <see langword="false"/>. </returns>
        /// <remarks> 
        /// If the deserialized element implements <see cref="IDeserializationCallback"/>,
        /// its deserialization callback <see cref="IDeserializationCallback.OnDeserialization"/> will be invoked.
        /// </remarks>
        protected bool TryDeserializeValue<T>(string name, out T elt, object sender)
        {
            try
            {
                elt = (T)this.sinfo.GetValue(name, typeof(T));
                InvokeDeserializationCallback(elt, sender);
                return true;
            }
            catch (SerializationException)
            {
                elt = default(T);
                return false;
            }
        }

        protected void AddAll(IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            foreach (KeyValuePair<TKey, TValue> pair in values)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        private static void ThrowWrongTypeArgumentException(object value, Type targetType, string name)
        {
            throw new ArgumentException(
                    string.Format(
                            CultureInfo.CurrentCulture,
                            "The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.",
                            value,
                            targetType),
                    name);
        }

        private static void VerifyKeyType(object key)
        {
            if (!(key is TKey))
            {
                ThrowWrongTypeArgumentException(key, typeof(TKey), "key");
            }
        }

        private static void VerifyValueType(object value)
        {
            if (!(value is TValue))
            {
                ThrowWrongTypeArgumentException(value, typeof(TValue), "value");
            }
        }
    }
}
