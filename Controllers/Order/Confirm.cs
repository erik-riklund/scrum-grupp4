using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public partial class OrderController : Controller
  {
    public IActionResult Confirm(string id)
    {
      // ATT GÖRA:
      // - hämta ordern och visa bekräftelse
      // - implementera en view.

      return View();
    }
  }
}