using App.Entities;
using App.Interfaces;
using MD5Hash;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using System.Security.Claims;

namespace App.Services
{
  public class Authenticator(IHttpContextAccessor context) : IAuthenticator
  {

    public async Task<bool> SignInAsync(string email, string password)
    {
      if (await ValidateCredentials(email, password) is User user)
      {
        var claims = new List<Claim> {
          new("ID", user.ID),
          new("ROLE", user.Roles.Any(role => role.Name.Equals("Admin")) ? "Admin":"Customer")
        };

        Debug.WriteLine(user.Roles.Any(role => role.Name.Equals("Admin")) ? "Y" : "N");

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
      await context.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private static async Task<User?> ValidateCredentials(string email, string password)
    {
      try
      {
        string passwordHash = password.GetMD5(EncodingType.UTF8);

        return await Query.FetchOne<User>(
          user => user.Email.Equals(email) && user.Password.Equals(passwordHash)
        );
      }

      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);

        return null;
      }
    }
  }
}
