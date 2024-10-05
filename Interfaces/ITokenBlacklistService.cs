namespace InventorizationBackend.Interfaces
{
  public interface ITokenBlacklistService
  {
    Task<bool> IsTokenBlacklisted(string jti);
    Task BlacklistToken(string jti, DateTime expiration);
  }
}
