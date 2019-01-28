﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Redis.Alpha
{
    /// <summary>
    /// Represents a manager for caching in Redis store (http://redis.io/).
    /// Mostly it'll be used when running in a web farm or Azure. But of course it can be also used on any server or environment
    /// </summary>
    public partial class RedisCacheManager
    {
        #region Fields

        private readonly RedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;

        #endregion

        #region Ctor

        public RedisCacheManager()
        {
            this._connectionWrapper = new RedisConnectionWrapper();
            this._db = this._connectionWrapper.GetDatabase();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <returns>The cached value associated with the specified key</returns>
        protected virtual async Task<T> GetAsync<T>(string key)
        {
            //get serialized item from cache
            var serializedItem = await _db.StringGetAsync(key);
            if (!serializedItem.HasValue)
                return default(T);

            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);

            return item;
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            //set cache item
            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);
            //and set it to cache
            await _db.StringSetAsync(key, serializedItem, expiresIn);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        protected virtual async Task<bool> IsSetAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param
        protected virtual async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Removes items by key pattern
        /// </summary>
        /// <param name="pattern">String key pattern</param>
        protected virtual async Task RemoveByPatternAsync(string pattern)
        {
            foreach (var endpoint in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(endpoint);
                var keys = server.Keys(database: _db.Database, pattern: $"*{pattern}*");

                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        protected virtual async Task ClearAsync()
        {
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(endPoint);
                //we manually delete all elements.
                var keys = server.Keys(_db.Database).ToArray();
                await _db.KeyDeleteAsync(keys);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            return GetAsync(key, () => Task.Run(acquire), cacheTime).Result;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
        {
            //item already is in cache, so return it
            if (await IsSetAsync(key))
                return await GetAsync<T>(key);
            //or create it using passed function
            var result = await acquire();
            //and set in cache 
            if (cacheTime != null)
                await SetAsync(key, result, cacheTime.Value);

            return result;
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        public virtual async void Set(string key, object data, int cacheTime)
        {
            await SetAsync(key, data, cacheTime);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        public virtual bool IsSet(string key)
        {
            return this.IsSetAsync(key).Result;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        public virtual async void Remove(string key)
        {
            await this.RemoveAsync(key);
        }

        /// <summary>
        /// Removes items by key pattern
        /// </summary>
        /// <param name="pattern">String key pattern</param>
        protected virtual async Task RemoveByPattern(string pattern)
        {
            await this.RemoveByPatternAsync(pattern);
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual async void Clear()
        {
            await this.ClearAsync();
        }

        #endregion
    }
}
