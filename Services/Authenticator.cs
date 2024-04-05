using System.Security.Claims;
using App.Entities;
using App.Interfaces;
using MD5Hash;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Entities;

namespace App.Services
{
  public class Authenticator(IHttpContextAccessor context) : IAuthenticator
  {
    private HttpContext Context { get; set; } = context.HttpContext
      ?? throw new ArgumentNullException(nameof(context));

    public async Task<bool> SignInAsync(string username, string password)
    {
      if (await ValidateCredentials(username, password) is string ID)
      {
        var claims = new List<Claim>{new ("ID", ID)};

        var identity = new ClaimsIdentity(
          claims, CookieAuthenticationDefaults.AuthenticationScheme
        );

        await context.HttpContext!.SignInAsync(
          CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)
        );

        return true;
      }

      return false;
    }

    public async Task SignOutAsync()
    {
      await Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private static async Task<string?> ValidateCredentials(string username, string password)
    {
      string passwordHash = password.GetMD5();
      var user = await Query.FetchOne<User>(user => user.UserName == username && user.Password == passwordHash);

      return user?.ID;
    }
  }
}
