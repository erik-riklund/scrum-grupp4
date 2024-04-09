using App.Entities;
using App.Interfaces;

namespace App.Services
{
  public class SessionManager(IHttpContextAccessor context) : ISessionManager
  {
    public async Task<User?> GetUserAsync()
    {
      if (context.HttpContext?.User.FindFirst("ID")?.Value is string ID)
      {
        return await Query.FetchOneById<User>(ID);
      }

      return null;
    }

    public bool IsLoggedIn() => context.HttpContext?.User.Identity?.IsAuthenticated == true;

    public bool IsAdmin() => IsLoggedIn() && (context.HttpContext?.User.Claims.Any(claim => claim.Type == "ROLE" && claim.Value == "Admin") ?? false);
  }
}
