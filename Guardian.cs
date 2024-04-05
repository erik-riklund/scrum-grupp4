using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace App
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class Guardian : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (!context.HttpContext.User.Identity!.IsAuthenticated) {
        context.Result = new RedirectResult("/Auth");
      }
    }
  }
}
