using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    public IActionResult Index() => View();
  }
}
