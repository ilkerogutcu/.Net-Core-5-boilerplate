using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Core.Utilities.IoC;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    /// <summary>
    /// Memory cache manager
    /// </summary>
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheManager"/> class.
        /// </summary>
        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        /// <summary>
        /// Add to cache
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="duration">Duration.</param>
        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        /// <summary>
        /// Get from cache
        /// </summary>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">Generic Class.</typeparam>
        /// <returns>Generic class.</returns>
        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        /// <summary>
        /// Get object from cache
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Object.</returns>
        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        /// <summary>
        /// Is add to cache control
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Boolean.</returns>
        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        /// <summary>
        /// Remove from the cache by key
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// Remove from the cache by pattern
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        public void RemoveByPattern(string pattern)
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition?.GetValue(_memoryCache) as dynamic;
            var cacheCollectionValues = new List<ICacheEntry>();

            if (cacheEntriesCollection != null)
            {
                foreach (var cacheItem in cacheEntriesCollection)
                {
                    ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                    cacheCollectionValues.Add(cacheItemValue);
                }
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues
                .Where(d => regex.IsMatch(d.Key.ToString() ?? string.Empty))
                .Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}