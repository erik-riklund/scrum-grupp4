using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    public IActionResult Index() => View();
  }
}
