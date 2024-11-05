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
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      var userInfo = await _authService.LoginAsync(model);

      if (string.IsNullOrEmpty(userInfo.token)) {
        return BadRequest("Invalid Credentials");
      }

      return Ok(userInfo);
    }

    [HttpPost("register")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
      if (model.Role != "ADMIN" && model.Role != "OPERATOR")
      {
        return BadRequest("Invalid role specified. Role must be either ADMIN or OPERATOR.");
      }

      var (user, Errors) = await _authService.RegisterAsync(model);

      if (user != null)
        return Ok(user);

      return BadRequest(new { errors = Errors });
    }
  }
}
