﻿using InventorizationBackend.Interfaces;
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
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      var token = await _authService.LoginAsync(model);

      if (string.IsNullOrEmpty(token))
        return Unauthorized();

      return Ok(new { accessToken = token });
    }
  }
}
