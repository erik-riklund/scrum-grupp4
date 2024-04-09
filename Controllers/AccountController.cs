using App.Entities;
using App.Interfaces;
using App.Models;
using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using System.Diagnostics;

namespace App.Controllers
{
  public class AccountController(IAuthenticator authenticator) : Controller
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
    [HttpGet]
    public IActionResult RegisterAdmin()
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

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
      try
      {
        await authenticator.SignOutAsync();

      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
      return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      // Step 1: Perform validation
      if (!ModelState.IsValid)
      {
        // Step 2: Add errors to ModelState
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
          // You can customize the error message here if needed
          ModelState.AddModelError(string.Empty, error.ErrorMessage);
        }
        // Step 3: Return to view with model
        return View(model);
      }
      try
      {
        var address = new Address { StreetAddress = model.StreetAddress, City = model.City, ZipCode = model.ZipCode, Country = model.Country };
        var user = new User { FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Phonenumber, Email = model.Email, Address = address, Password = model.Password.GetMD5() };
        await user.SaveAsync();

      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);

      }


      return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel model)
    {

      if (!ModelState.IsValid)
      {
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {

          ModelState.AddModelError(string.Empty, error.ErrorMessage);
        }

        return View(model);
      }
      try
      {
        var adminRole = await Query.FetchOne<Role>(role => role.Name == "Admin");

        if (adminRole == null)
        {
          adminRole = new Role { Name = "Admin" };
          await adminRole.SaveAsync();
        }

        var user = new User { FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Phonenumber, Email = model.Email, Password = model.Password.GetMD5() };
        await user.SaveAsync();

        await user.Roles.AddAsync(adminRole);

      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);

      }

      return RedirectToAction("Index", "Home");
    }


  }

}
