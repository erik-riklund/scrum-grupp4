using App.Entities;
using App.Interfaces;

namespace App.Services
{
  public class SessionManager(IHttpContextAccessor context) : ISessionManager
  {
    private HttpContext Context { get; set; } = context.HttpContext
      ?? throw new ArgumentNullException(nameof(context));

    public async Task<User?> GetUserAsync()
    {
      if (Context.User.FindFirst("ID")?.Value is string ID)
      {
        return await Query.FetchOneById<User>(ID);
      }

      return null;
    }

    public bool IsAuthenticated() => Context.User.Identity?.IsAuthenticated == true;

    public async Task<bool> HasRoleAsync(string roleName) =>
      (await GetUserAsync())?.Roles.Where(role => role.Name.Equals(roleName)).Any() == true;
  }
}
