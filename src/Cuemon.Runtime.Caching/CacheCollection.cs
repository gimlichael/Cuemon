using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cuemon.Collections.Generic;

namespace Cuemon.Runtime.Caching
{
    /// <summary>
    /// Implements a cache for an application. This class cannot be inherited.
    /// </summary>
    public sealed partial class CacheCollection : IEnumerable<KeyValuePair<long, object>>
    {
        private static readonly CacheCollection Singleton = new CacheCollection();
        private readonly ConcurrentDictionary<long, Cache> _innerCaches = new ConcurrentDictionary<long, Cache>();
        private const string NoGroup = null;

        internal static CacheCollection Cache
        {
            get { return Singleton; }
        }

        #region Constructors
        private CacheCollection()
        {
            EnableExpirationTimer = true;
            ExpirationTimer = new Timer(ExpirationTimerInvoking, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(20));
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether a timer regularly should clean up expired cache items.
        /// </summary>
        /// <value><c>true</c> if a timer regularly should clean up expired cache items; otherwise, <c>false</c>.</value>
        public bool EnableExpirationTimer { get; set; }

        /// <summary>
        /// Gets the cached item with the specified <paramref name="key"/>.
        /// </summary>
        /// <value>The cached item matching the specified <paramref name="key"/>.</value>
        public object this[string key]
        {
            get
            {
                return this[key, NoGroup];
            }
        }

        /// <summary>
        /// Gets the cached item with the specified <paramref name="key"/> and <paramref name="group"/>.
        /// </summary>
        /// <value>The cached item matching the specified <paramref name="key"/> and <paramref name="group"/>.</value>
        public object this[string key, string group]
        {
            get
            {
                return Get<object>(key, group);
            }
        }

        private Timer ExpirationTimer { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Retrieves the specified item from the <see cref="CacheCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item in the <see cref="CacheCollection"/>.</typeparam>
        /// <param name="key">The identifier of the cache item to retrieve.</param>
        /// <returns>The retrieved cache item, or the default value of the type parameter T if the key is not found.</returns>
        public T Get<T>(string key)
        {
            return Get<T>(key, NoGroup);
        }

        /// <summary>
        /// Retrieves the specified item from the associated group of the <see cref="CacheCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item in the <see cref="CacheCollection"/>.</typeparam>
        /// <param name="key">The identifier of the cache item to retrieve.</param>
        /// <param name="group">The associated group of the cache item to retrieve.</param>
        /// <returns>The retrieved cache item, or the default value of the type parameter T if the key is not found.</returns>
        public T Get<T>(string key, string group)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            T result;
            TryGetValue(key, group, out result);
            return result;
        }

        /// <summary>
        /// Updates the specified item from the associated group of the <see cref="CacheCollection" />.
        /// </summary>
        /// <typeparam name="T">The type of the item in the <see cref="CacheCollection" />.</typeparam>
        /// <param name="key">The identifier of the cache item to retrieve and update with <paramref name="value"/>.</param>
        /// <param name="group">The associated group of the cache item to retrieve and update with <paramref name="value"/>.</param>
        /// <param name="value">The value to apply to the cached item.</param>
        internal void Set<T>(string key, string group, T value)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            Cache cache;
            if (TryGetCache(key, group, out cache))
            {
                cache.Value = value;
                cache.Refresh();
            }
        }

        /// <summary>
        /// Gets the UTC date time value from when this item was added to the <see cref="CacheCollection"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the UTC date time value from when this item, with the specified key, was added; otherwise, if no item could be resolved or the item has expired, <see cref="DateTime.MinValue"/>. This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> parameter contains an element with the specified key, and the element has not expired; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool TryGetAdded(string key, out DateTime value)
        {
            return TryGetAdded(key, NoGroup, out value);
        }

        /// <summary>
        /// Gets the UTC date time value from when this item was added to the <see cref="CacheCollection"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="group">The group of the value to get.</param>
        /// <param name="value">When this method returns, contains the UTC date time value from when this item, with the specified key, was added; otherwise, if no item could be resolved or the item has expired, <see cref="DateTime.MinValue"/>. This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> parameter contains an element with the specified key, and the element has not expired; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool TryGetAdded(string key, string group, out DateTime value)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            Cache cache;
            if (TryGetCache(key, group, out cache))
            {
                value = cache.Created;
                return true;
            }
            value = DateTime.MinValue;
            return false;
        }

        private static long GenerateGroupKey(string key, string group)
        {
            return StructUtility.GetHashCode64(group == NoGroup ? key : string.Concat(key, group));
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value)
        {
            Add(key, value, NoGroup);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.up.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="group">The group to associate the <paramref name="key"/> with.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, string group)
        {
            AddCore(key, value, group, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="absoluteExpiration">The time at which the added object expires and is removed from the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, DateTime absoluteExpiration)
        {
            Add(key, value, NoGroup, absoluteExpiration);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="group">The group to associate the <paramref name="key"/> with.</param>
        /// <param name="absoluteExpiration">The time at which the added object expires and is removed from the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, string group, DateTime absoluteExpiration)
        {
            AddCore(key, value, group, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="slidingExpiration">The interval between the time the added object was last accessed and the time at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed from the cache 20 minutes after it is last accessed.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, TimeSpan slidingExpiration)
        {
            Add(key, value, NoGroup, slidingExpiration);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="group">The group to associate the <paramref name="key"/> with.</param>
        /// <param name="slidingExpiration">The interval between the time the added object was last accessed and the time at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, string group, TimeSpan slidingExpiration)
        {
            AddCore(key, value, group, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="dependencies">The dependencies for the <paramref name="value"/>. When any dependency changes, the <paramref name="value"/> becomes invalid and is removed from the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, params IDependency[] dependencies)
        {
            Add(key, value, NoGroup, dependencies);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="dependencies">The dependencies for the <paramref name="value"/>. When any dependency changes, the <paramref name="value"/> becomes invalid and is removed from the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, IEnumerable<IDependency> dependencies)
        {
            Add(key, value, NoGroup, dependencies);
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="group">The group to associate the <paramref name="key"/> with.</param>
        /// <param name="dependencies">The dependencies for the <paramref name="value"/>. When any dependency changes, the <paramref name="value"/> becomes invalid and is removed from the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, string group, params IDependency[] dependencies)
        {
            Add(key, value, group, EnumerableConverter.FromArray(dependencies));
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and <paramref name="value"/> to the cache.
        /// </summary>
        /// <param name="key">The cache key used to identify the item.</param>
        /// <param name="value">The object to be inserted in the cache.</param>
        /// <param name="group">The group to associate the <paramref name="key"/> with.</param>
        /// <param name="dependencies">The dependencies for the <paramref name="value"/>. When any dependency changes, the <paramref name="value"/> becomes invalid and is removed from the cache.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <remarks>
        /// This method will not throw an <see cref="ArgumentException"/> in case of an existing cache item whose key matches the key parameter.
        /// </remarks>
        public void Add(string key, object value, string group, IEnumerable<IDependency> dependencies)
        {
            AddCore(key, value, group, DateTime.MaxValue, TimeSpan.Zero, dependencies);
        }

        private void AddCore(string key, object value, string group, DateTime absoluteExpiration, TimeSpan slidingExpiration, IEnumerable<IDependency> dependencies)
        {
            var f1 = DoerFactory.Create(() => value);
            var f2 = DoerFactory.Create(() => dependencies);
            GetOrAddCore(f1, key, group, () => absoluteExpiration, () => slidingExpiration, f2);
        }

        private void ExpirationTimerInvoking(object o)
        {
            HandleExpiration();
        }

        private void RemoveExpired(string key, string group)
        {
            Remove(key, group);
        }

        private void HandleExpiration()
        {
            if (!EnableExpirationTimer) { return; }
            DateTime current = DateTime.UtcNow;
            List<Cache> snapshot = new List<Cache>(_innerCaches.Values);
            if (snapshot.Count > 0)
            {
                foreach (Cache cache in snapshot)
                {
                    if (cache == null) { continue; }
                    if (cache.CanExpire && cache.HasExpired(current)) { RemoveExpired(cache.Key, cache.Group); }
                }
            }
        }

        private void CacheExpired(object sender, CacheEventArgs e)
        {
            RemoveExpired(e.Cache.Key, e.Cache.Group);
            e.Cache.Expired -= CacheExpired;
        }

        private IList<Cache> GetCaches(string group)
        {
            var current = DateTime.UtcNow;
            var groupCaches = new List<Cache>();
            var snapshot = new List<Cache>(_innerCaches.Values);
            foreach (Cache cache in snapshot)
            {
                if (cache == null) { continue; } // this can happen if a cache has been removed
                if (cache.CanExpire && cache.HasExpired(current)) { continue; }
                if (group == NoGroup)
                {
                    groupCaches.Add(cache); // return all
                }
                else if (cache.Group == group)
                {
                    groupCaches.Add(cache); // return filtered by group
                }
            }
            return groupCaches;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="T:System.Collections.Generic.Dictionary`2"/> contains an element with the specified key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool ContainsKey(string key)
        {
            return ContainsKey(key, NoGroup);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param>
        /// <param name="group">The associated group of the key to locate.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="T:System.Collections.Generic.Dictionary`2"/> contains an element with the specified key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool ContainsKey(string key, string group)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            Cache cache;
            return TryGetCache(key, group, out cache);
        }

        /// <summary>
        /// Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </summary>
        public void Clear()
        {
            Clear(NoGroup);
        }

        /// <summary>
        /// Removes all keys and values matching the specified group from the <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </summary>
        public void Clear(string group)
        {
            if (group == NoGroup)
            {
                _innerCaches.Clear();
            }
            else
            {
                IList<Cache> groupCaches = GetCaches(group);
                foreach (var cache in groupCaches)
                {
                    Remove(cache.Key, cache.Group);
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count()
        {
            return Count(NoGroup);
        }

        /// <summary>
        /// Gets the number of elements contained in the specified group of the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="group">The associated group to filter the count by.</param>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <value></value>
        public int Count(string group)
        {
            int count = GetCaches(group).Count;
            return count;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the item in the <see cref="CacheCollection"/>.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.Dictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool TryGetValue<T>(string key, out T value)
        {
            return TryGetValue(key, NoGroup, out value);
        }

        /// <summary>
        /// Gets the value associated with the specified key and group.
        /// </summary>
        /// <typeparam name="T">The type of the item in the <see cref="CacheCollection"/>.</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="group">The group of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.Dictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool TryGetValue<T>(string key, string group, out T value)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            Cache cache;
            if (TryGetCache(key, group, out cache))
            {
                value = (T)cache.Value;
                return true;
            }
            value = default(T);
            return false;
        }

        private bool TryGetCache(string key, string group, out Cache cache)
        {
            DateTime current = DateTime.UtcNow;
            bool hasItem;
            long groupKey = GenerateGroupKey(key, group);
            hasItem = _innerCaches.TryGetValue(groupKey, out cache);
            if (hasItem)
            {
                if (cache.CanExpire && !cache.HasExpired(current))
                {
                    cache.Refresh();
                    return true;
                }

                if (cache.CanExpire && cache.HasExpired(current))
                {
                    RemoveExpired(key, group);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully found and removed; otherwise, false.  This method returns false if <paramref name="key"/> is not found in the <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool Remove(string key)
        {
            return Remove(key, NoGroup);
        }

        /// <summary>
        /// Removes the value with the specified key from the associated specified group of the <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="group">The associated group to the key of the element to remove.</param>
        /// <returns>
        /// <c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>.  This method returns <c>false</c> if <paramref name="key"/> combined with <paramref name="group"/>  is not found in the <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.
        /// </exception>
        public bool Remove(string key, string group)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            long groupKey = GenerateGroupKey(key, group);
            Cache ignore;
            return _innerCaches.TryRemove(groupKey, out ignore);
        }

        private IEnumerable<KeyValuePair<long, object>> CreateImpostor()
        {
            foreach (var keyValuePair in _innerCaches)
            {
                if (keyValuePair.Value != null)
                {
                    yield return new KeyValuePair<long, object>(keyValuePair.Key, keyValuePair.Value.Value);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Retrieves an enumerator that iterates through the key settings and their values contained in the cache.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        /// <remarks>All keys are hashed internally and will not provide useful information.</remarks>
        public IEnumerator<KeyValuePair<long, object>> GetEnumerator()
        {
            List<KeyValuePair<long, object>> impostor = new List<KeyValuePair<long, object>>(CreateImpostor());
            return impostor.GetEnumerator();
        }
        #endregion
    }
}