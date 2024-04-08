using System.Diagnostics;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public class HomeController : Controller
  {
    public async Task<IActionResult> Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
