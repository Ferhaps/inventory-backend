using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventorizationBackend.Services
{
  public class UserService(UserManager<ApplicationUser> userManager) : IUserService
  {
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<List<UserDto>> GetUsersAsync()
    {
      var users = await _userManager.Users.ToListAsync();
      var userDtos = new List<UserDto>();

      if (users == null)
      {
        throw new ArgumentNullException(nameof(users));
      }
      else
      {
        foreach (var user in users)
        {
          var userRoles = await _userManager.GetRolesAsync(user);
          userDtos.Add(new UserDto
          {
             Id = user.Id,
             Email = user.Email,
             Role = userRoles.FirstOrDefault() ?? "OPERATOR"
          });
        }
      }

      return userDtos;
    }
    public async Task<bool> DeleteUserAsync(string userId)
    {
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return false;
      }

      var result = await _userManager.DeleteAsync(user);
      if (!result.Succeeded)
      {
        return false;
      }

      return true;
    }
  }
}
