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
      var token = await _authService.LoginAsync(model);

      if (string.IsNullOrEmpty(token))
        return BadRequest(new { error = "Invalid Credentials" });

      return Ok(new { accessToken = token });
    }

    [HttpPost("register")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {
      if (model.Role != "ADMIN" && model.Role != "OPERATOR")
      {
        return BadRequest(new { error = "Invalid role specified. Role must be either ADMIN or OPERATOR." });
      }

      var (Succeeded, Errors) = await _authService.RegisterAsync(model);

      if (Succeeded)
        return Ok(new { error = "User registered successfully" });

      return BadRequest(new { errors = Errors });
    }
  }
}
