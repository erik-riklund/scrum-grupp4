using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
    public partial class ModelController : Controller
    {
        public async Task<IActionResult> HandleModels()
        {
            var cursor = await DB.Find<Model>().ExecuteCursorAsync();
            var models = await cursor.ToListAsync();
            ViewBag.CurrentModels = models.Where(m => !m.ModelName.Equals("Specialhat")).ToList();

            var cursor2 = await DB.Find<Material>().ExecuteCursorAsync();
            ViewBag.CurrentMaterials = await cursor2.ToListAsync();

            return View();
        }
    }
}