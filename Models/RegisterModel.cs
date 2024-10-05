using System.ComponentModel.DataAnnotations;

namespace InventorizationBackend.Models
{
  public class RegisterModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [RegularExpression("^(ADMIN|OPERATOR)$", ErrorMessage = "Role must be either ADMIN or OPERATOR")]
    public string Role { get; set; }
  }
}
