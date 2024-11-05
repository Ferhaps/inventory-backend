using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthController(IAuthService authService) : ControllerBase
  {
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(LoggedUserDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      var userInfo = await _authService.LoginAsync(model);

      if (userInfo == null)
      {
        return BadRequest("Invalid Credentials");
      }

      return Ok(userInfo);
    }

    [HttpPost("register")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      var (user, Errors) = await _authService.RegisterAsync(model);

      if (user == null)
      {
        return BadRequest(new { errors = Errors });
      }

      return Ok(user);
    }
  }
}
