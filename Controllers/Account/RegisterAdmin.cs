using App.Entities;
using App.Interfaces;
using App.Models;
using MD5Hash;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using System.Diagnostics;

namespace App.Controllers
{
  public partial class AccountController : Controller
  {
    [HttpGet]
    public IActionResult RegisterAdmin() => View();

    [HttpPost]
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
                var emailCheck = await Query.FetchOne<User>(user => user.Email.Equals(model.Email));
                if (emailCheck != null)
                {
                    ModelState.AddModelError(string.Empty, "The email already exist choose a new email");
                    return View(model);

                }
                else 
                {
                    var user = new User { FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.Phonenumber, Email = model.Email, Password = model.Password.GetMD5() };
                    await user.SaveAsync();
                    await user.Roles.AddAsync(adminRole);
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