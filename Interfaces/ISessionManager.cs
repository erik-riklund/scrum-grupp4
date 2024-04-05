using App.Entities;

namespace App.Interfaces
{
  public interface ISessionManager
  {
    /// <summary>Retrieve the entity for the currently active user.</summary>
    Task<User?> GetUserAsync();

    /// <summary>Check if the current user have the specified role.</summary>
    /// <param name="roleName">The name of the role.</param>
    Task<bool> HasRoleAsync(string roleName);

    /// <summary>Check whether the user is authenticated or not.</summary>
    bool IsAuthenticated();
  }
}
