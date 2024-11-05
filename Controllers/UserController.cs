using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/users")]
  [ApiController]
  public class UserController(IUserService userService) : ControllerBase
  {
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetUsers()
    {
      try
      {
        var users = await _userService.GetUsersAsync();

        if (users == null)
        {
          return BadRequest();
        }

        return Ok(users);

      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser(string id)
    {
      var success = await _userService.DeleteUserAsync(id);

      if (success) 
      {
        return Ok();
      }

      return NotFound();
    }
  }
}
