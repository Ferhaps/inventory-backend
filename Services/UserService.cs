using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace InventorizationBackend.Services
{
  public class UserService(UserManager<ApplicationUser> userManager) : IUserService
  {
    private readonly UserManager<ApplicationUser> _userManager = userManager;
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
