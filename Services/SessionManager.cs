using App.Entities;

namespace App.Services
{
  public class SessionManager(IHttpContextAccessor context)
  {
    private HttpContext Context { get; set; } = context.HttpContext
      ?? throw new ArgumentNullException(nameof(context));

    public async Task<User?> GetUser()
    {
      if (Context.User.FindFirst("ID")?.Value is string ID)
      {
        return await Query.FetchOneById<User>(ID);
      }

      return null;
    }

    public bool IsAuthenticated() => Context.User.Identity?.IsAuthenticated == true;

    public async Task<bool> HasRole(string roleName) =>
      (await GetUser())?.Roles.Where(role => role.Name.Equals(roleName)).Any() == true;
  }
}
