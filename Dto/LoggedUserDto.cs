namespace InventorizationBackend.Dto
{
  public class LoggedUserDto
  {
    public string token { get; set; }

    public UserDto User { get; set; }
  }
}
