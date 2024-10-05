using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IAuthService
  {
    Task<string> LoginAsync(LoginModel loginBody);
    Task<(bool Succeeded, string[] Errors)> RegisterAsync(RegisterModel registerBody);
  }
}
