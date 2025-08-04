using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using System.Collections.Generic; // Added for List

namespace Asasy.Service.Common
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? slidingExpiration = null);
        void Remove(string key);
        void RemoveByPattern(string pattern);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? slidingExpiration = null)
        {
            return await _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                entry.SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(30);
                return await getItem();
            });
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            // This is a simplified implementation
            // In production, consider using IMemoryCache with custom implementation
            // or use Redis with pattern-based removal
            if (_memoryCache is MemoryCache mc)
            {
                var field = typeof(MemoryCache).GetField("_coherentState", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field?.GetValue(mc) is object coherentState)
                {
                    var entriesCollection = coherentState.GetType()
                        .GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (entriesCollection?.GetValue(coherentState) is System.Collections.IDictionary entries)
                    {
                        var keysToRemove = new List<object>();
                        foreach (System.Collections.DictionaryEntry entry in entries)
                        {
                            if (entry.Key.ToString().Contains(pattern))
                            {
                                keysToRemove.Add(entry.Key);
                            }
                        }
                        
                        foreach (var key in keysToRemove)
                        {
                            _memoryCache.Remove(key);
                        }
                    }
                }
            }
        }
    }
}