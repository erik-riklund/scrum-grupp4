using App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.Controllers
{
  public partial class AccountController : Controller
  {
    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var username = model.userName;
          var password = model.password;
          if (await authenticator.SignInAsync(username, password))
          {
            if (returnUrl != null)
            {
              return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
          }
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }

      return View();
    }
  }
}