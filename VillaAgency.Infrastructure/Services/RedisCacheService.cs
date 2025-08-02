using StackExchange.Redis;
using System.Text.Json;
using VillaAgency.Application.Common.Interfaces.Services;

namespace VillaAgency.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _db=connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public async Task<bool> RemoveDataAsync(string key)
        {
            var keyExists = await _db.KeyExistsAsync(key);
            if (keyExists)
            {
                return await _db.KeyDeleteAsync(key);
            }
            return false;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expirationTime)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            var isSet = await _db.StringSetAsync(key, serializedValue, expirationTime);
            return isSet;

        }
    }
}
