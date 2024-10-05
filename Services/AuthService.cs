using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventorizationBackend.Services
{
  public class AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IAuthService
  {
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> LoginAsync(LoginModel loginBody)
    {
      var user = await _userManager.FindByEmailAsync(loginBody.Email);
      if (user != null && await _userManager.CheckPasswordAsync(user, loginBody.Password))
      {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var auds = _configuration.GetSection("JWT:Audiences").Get<string[]>()!;
        foreach (var aud in auds)
        {
          Console.WriteLine(aud);
          authClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, aud));
        }

        foreach (var userRole in userRoles)
        {
          authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
          issuer: _configuration["JWT:Issuer"],
          audience: null,
          expires: DateTime.Now.AddHours(3),
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
      }

      return null;
    }

    public async Task<(bool Succeeded, string[] Errors)> RegisterAsync(RegisterModel registerBody)
    {
      var user = new ApplicationUser { UserName = registerBody.Email, Email = registerBody.Email };
      var result = await _userManager.CreateAsync(user, registerBody.Password);

      if (result.Succeeded)
      {
        if (registerBody.Role != "ADMIN" && registerBody.Role != "OPERATOR")
        {
          return (false, new[] { "Invalid role specified." });
        }

        if (!await _roleManager.RoleExistsAsync(registerBody.Role))
        {
          await _roleManager.CreateAsync(new IdentityRole(registerBody.Role));
        }

        await _userManager.AddToRoleAsync(user, registerBody.Role);

        return (true, []);
      }

      return (false, result.Errors.Select(e => e.Description).ToArray());
    }
  }
}
