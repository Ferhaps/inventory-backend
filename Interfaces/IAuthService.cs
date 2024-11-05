using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IAuthService
  {
    Task<LoggedUserDto> LoginAsync(LoginModel loginBody);
    Task<(UserDto? user, string[] Errors)> RegisterAsync(RegisterModel registerBody);
  }
}
