namespace InventorizationBackend.Interfaces
{
  public interface IUserService
  {
    Task<bool> DeleteUserAsync(string userId);
  }
}
