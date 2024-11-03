using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IAuthService
  {
    Task<LoggedUserDto> LoginAsync(LoginModel loginBody);
    Task<(bool Succeeded, string[] Errors)> RegisterAsync(RegisterModel registerBody);
  }
}
