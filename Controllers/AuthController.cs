using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public class AuthController(IHttpContextAccessor context) : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> Login()
    {
      var claims = new List<Claim>
      {
        new (ClaimTypes.Name, "Gopher"),
        new ("FullName", "Erik Riklund"),
        new (ClaimTypes.Role, "User")
      };

      var identity = new ClaimsIdentity(
        claims, CookieAuthenticationDefaults.AuthenticationScheme
      );

      await context.HttpContext!.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)
      );

      return RedirectToAction("Protected");
    }

    public IActionResult LoginAdmin()
    {
      return RedirectToAction("Admin");
    }

    public async Task<IActionResult> Logout()
    {
      await context.HttpContext!.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme
      );

      return RedirectToAction("Index");
    }

    [Guardian]
    public IActionResult Protected()
    {
      ViewBag.Name = context.HttpContext!.User.Identity?.Name;

      return View();
    }

    public IActionResult Admin()
    {
      return View();
    }
  }
}
