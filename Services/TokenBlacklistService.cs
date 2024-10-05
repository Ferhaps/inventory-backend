using InventorizationBackend.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace InventorizationBackend.Services
{
  public class TokenBlacklistService(IMemoryCache cache) : ITokenBlacklistService
  {
    private readonly IMemoryCache _cache = cache; // Could be a database or Redis for production

    public Task<bool> IsTokenBlacklisted(string jti)
    {
      return Task.FromResult(_cache.TryGetValue(jti, out _));
    }

    public Task BlacklistToken(string jti, DateTime expiration)
    {
      _cache.Set(jti, true, expiration); // Cache the JTI until the token's expiration time
      return Task.CompletedTask;
    }
  }
}
