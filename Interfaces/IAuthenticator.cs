namespace App.Interfaces
{
  public interface IAuthenticator
  {
    /// <summary>Validate the provided credentials and create a new user session.</summary>
    Task<bool> SignInAsync(string username, string password);

    /// <summary>Destroy the current user session.</summary>
    Task SignOutAsync();
  }
}
