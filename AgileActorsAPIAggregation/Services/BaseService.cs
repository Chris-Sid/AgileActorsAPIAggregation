using AgileActorsAPIAggregation.Helpers;
using AgileActorsAPIAggregation.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace AgileActorsAPIAggregation.Services
{
    public abstract class BaseService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IMemoryCache _cache;

        protected BaseService(IHttpClientWrapper httpClientWrapper, IMemoryCache cache)
        {
            _httpClientWrapper = httpClientWrapper;
            _cache = cache;
        }

        protected async Task<T> GetFromCacheOrFetchAsync<T>(string cacheKeyBase, string apiUrl, Func<T> fallback)
        {
            string uniqueCacheKey = $"{cacheKeyBase}_{apiUrl.GetHashCode()}";

            if (!_cache.TryGetValue(uniqueCacheKey, out CachedDataWrapper<T> cachedDataWrapper))
            {
                //Cache miss
                try
                {
                    var data = await _httpClientWrapper.GetFromJsonAsync<T>(apiUrl);
                    cachedDataWrapper = new CachedDataWrapper<T>(data, GetHash(data));
                    _cache.Set(uniqueCacheKey, cachedDataWrapper, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
                   // Cached new data
                }
                catch (HttpRequestException)
                {
                    var fallbackData = fallback();
                    var fallbackDataHash = GetHash(fallbackData);
                    cachedDataWrapper = new CachedDataWrapper<T>(fallbackData, fallbackDataHash);
                   //Using fallback data
                }
            }

            // Fetch new data and generate its hash key
            var newData = await _httpClientWrapper.GetFromJsonAsync<T>(apiUrl);
            var newDataHash = GetHash(newData);

            // Compare hash keys
            if (cachedDataWrapper.Hash != newDataHash)
            {
                cachedDataWrapper = new CachedDataWrapper<T>(newData, newDataHash);
                _cache.Set(uniqueCacheKey, cachedDataWrapper, TimeSpan.FromMinutes(10));
                Console.WriteLine("Cache updated with new data");
            }
            else
            {
                Console.WriteLine("Cache data is still valid");
            }

            if (cachedDataWrapper.Data == null)
            {
                var fallbackData = fallback();
                var fallbackDataHash = GetHash(fallbackData);
                cachedDataWrapper = new CachedDataWrapper<T>(fallbackData, fallbackDataHash);
            }

            return cachedDataWrapper.Data;
        }


        public class CachedDataWrapper<T>
        {
            public T Data { get; set; }
            public string Hash { get; set; }

            public CachedDataWrapper(T data, string hash)
            {
                Data = data;
                Hash = hash;
            }
        }
        private string GetHash<T>(T obj)
        {
            if (obj == null) return null;

            var json = JsonSerializer.Serialize(obj);
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                return Convert.ToBase64String(bytes);
            }
        }

    }
}
