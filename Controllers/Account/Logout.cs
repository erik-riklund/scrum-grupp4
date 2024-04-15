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
  }
}