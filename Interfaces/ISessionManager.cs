using App.Entities;

namespace App.Interfaces
{
  public interface ISessionManager
  {
    /// <summary>Retrieve the entity for the currently active user.</summary>
    Task<User?> GetUserAsync();

    /// <summary>Check whether the user is authenticated or not.</summary>
    bool IsLoggedIn();

    /// <summary>Check whether the user is an admin.</summary>
    bool IsAdmin();
  }
}
