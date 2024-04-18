using App.Entities;
using App.Interfaces;
using App.Models;
using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Controllers
{
  public partial class AccountController : Controller
  {
    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
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
                var emailCheck = await Query.FetchOne<User>(user => user.Email.Equals(model.Email));
                if (emailCheck ==null)
                {
                    var address = new Address { StreetAddress = model.StreetAddress, City = model.City, ZipCode = model.ZipCode, Country = model.Country };
                    var user = new User { FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Phonenumber, Email = model.Email, Address = address, Password = model.Password.GetMD5() };
                    await user.SaveAsync();

                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "The email already exist choose a new email");
                    return View(model);
                }


            }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);

      }


      return RedirectToAction("Index", "Home");
    }
  }
}