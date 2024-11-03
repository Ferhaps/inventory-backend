using InventorizationBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/users")]
  [ApiController]
  public class UserController(IUserService userService) : ControllerBase
  {
    private readonly IUserService _userService = userService;

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
      var success = await _userService.DeleteUserAsync(id);

      if (success) 
      {
        return Ok();
      }

      return BadRequest();
    }
  }
}
