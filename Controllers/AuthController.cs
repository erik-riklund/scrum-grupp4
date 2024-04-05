using System.Security.Claims;
using App.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public class AuthController(IHttpContextAccessor context, Authenticator authenticator, SessionManager activeUser) : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> Login()
    {
      if (await authenticator.SignInAsync("Gopher","test"))
      {
        return RedirectToAction("Protected");
      }

      return RedirectToAction("Auth");
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
    public async Task<IActionResult> Protected()
    {
      ViewBag.Name = (await activeUser.GetUser())?.UserName ?? "Namn saknas";

      return View();
    }

    [Guardian(Roles = "Admin")]
    public IActionResult Admin()
    {
      return View();
    }
  }
}
