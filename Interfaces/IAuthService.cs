using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IAuthService
  {
    Task<string> LoginAsync(LoginModel loginBody);
  }
}
