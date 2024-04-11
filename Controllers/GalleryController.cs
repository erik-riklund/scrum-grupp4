using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public class GalleryController : Controller
  {
    public IActionResult Gallery() => View();
  }
}