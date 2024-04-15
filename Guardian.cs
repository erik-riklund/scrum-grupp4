using App.Entities;
using App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver.Linq;
using System.Security.Claims;

namespace App
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class Guardian : Attribute, IAuthorizationFilter
  {
    public string Roles { get; set; } = null!;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (context.HttpContext is var request)
      {
        if (request.User is ClaimsPrincipal claims)
        {
          if (claims.FindFirst("ID")?.Value is string ID)
          {
            if (context.HttpContext?.User.Identity?.IsAuthenticated == true)
            {
              if (string.IsNullOrEmpty(Roles)) return; // no role check

              if (context.HttpContext?.User.Claims.Any(claim => claim.Type == "ROLE" && claim.Value == "Admin") == true)
              {
                return; // admin privileges found!
              }
            }
          }
        }
      }

      string returnTo = context.HttpContext?.Request.Path.ToString() ?? "/";
      string encodedReturnTo = Uri.EscapeDataString(returnTo);

      context.Result = new RedirectResult($"/Account/Login?returnUrl={encodedReturnTo}");
    }
  }
}
