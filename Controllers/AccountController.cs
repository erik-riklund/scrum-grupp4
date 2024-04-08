using Microsoft.AspNetCore.Mvc;
using App.Services;
using System.Diagnostics;
 using App.Models;

namespace App.Controllers
{
    public class AccountController (Authenticator authenticator) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, String returnUrl)
        {
            try {
                if (ModelState.IsValid)
                {
                    var username = model.userName;
                    var password = model.password;
                    if (await authenticator.SignInAsync(username, password))
                    {
                        if(returnUrl !=null)
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

        [HttpGet]
        public async Task<IActionResult> LogOut ()
        {
            try
            {
                await authenticator.SignOutAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return RedirectToAction ("Index","Home");
        }

    }

}
