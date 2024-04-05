using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Entities;
using System.Diagnostics;
using App.Entities;
using MongoDB.Driver.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace App
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class Guardian : Attribute, IAsyncAuthorizationFilter
  {
    public string Roles { get; set; } = null!;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      //Debug.WriteLine("Legitimation, tack!");

      if (context.HttpContext is var request)
      {
        //Debug.WriteLine("Förfrågan fifan...");

        if (request.User is ClaimsPrincipal claims)
        {
          //Debug.WriteLine("Användare lokaliserad.");
          //Debug.WriteLine(claims.FindFirst("ID"));

          if (claims.FindFirst("ID")?.Value is string ID)
          {
            if (string.IsNullOrEmpty(Roles)) return; // no role check

            if (await Query.FetchOneById<User>(ID) is User user)
            {
              //Debug.WriteLine("Användare laddad från databasen.");

              var requiredRoles = Roles.Split(',', StringSplitOptions.TrimEntries).ToList();
              if (user.Roles.Where(role => requiredRoles.Contains(role.Name)).Any()) return;
            }
          }
        }
      }

      context.Result = new RedirectResult("/Auth");
    }
  }
}
