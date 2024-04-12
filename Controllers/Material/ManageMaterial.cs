using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> ManageMaterial()
    {
            var cursor = await DB.Find<Supplier>().ExecuteCursorAsync();
            ViewBag.CurrentSuppliers = await cursor.ToListAsync();

            return View();
    }
  }
}
