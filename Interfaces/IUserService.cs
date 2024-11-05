using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IUserService
  {
    Task<bool> DeleteUserAsync(string userId);

    Task<List<UserDto>> GetUsersAsync();
  }
}
