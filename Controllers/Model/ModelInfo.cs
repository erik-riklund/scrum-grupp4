using Microsoft.AspNetCore.Mvc;
using App.Entities;

namespace App.Controllers
{
    public partial class ModelController : Controller
    {
        public async Task<IActionResult> ModelInfo(String modelID) 
        {
            var model = await Query.FetchOneById<Model>(modelID);
            return View(model);
        }
    }
}
