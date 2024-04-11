using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public partial class AdminController
  {
    public IActionResult Index() => View();
  }
}