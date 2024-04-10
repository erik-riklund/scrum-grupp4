using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class ModelController : Controller
  {
    public async Task<IActionResult> HandleModels()
    {
      var cursor = await DB.Find<Model>().ExecuteCursorAsync();
      ViewBag.CurrentModels = await cursor.ToListAsync();

      var cursor2 = await DB.Find<Material>().ExecuteCursorAsync();
      ViewBag.CurrentMaterials = await cursor2.ToListAsync();

      return View();
    }
  }
}