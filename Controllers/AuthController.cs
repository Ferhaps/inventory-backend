using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
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
  }
}
