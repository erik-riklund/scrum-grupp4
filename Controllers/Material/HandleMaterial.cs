using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> ManageMaterial()
    {
      return View();
    }
  }
}
