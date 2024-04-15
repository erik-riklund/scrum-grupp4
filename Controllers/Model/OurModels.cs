using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using App.Entities;

namespace App.Controllers
{
    public partial class ModelController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> OurModels()
        {
            var models = await Query.FetchAll<Model>();
            List<Model> modelList = models.Where(m => m.ModelName != "Specialhat").ToList();
            return View(modelList);
        }
    }
}
