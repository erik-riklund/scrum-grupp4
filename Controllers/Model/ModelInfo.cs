using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Models;

namespace App.Controllers
{
    public partial class ModelController : Controller
    {
        public async Task<IActionResult> ModelInfo(String modelID) 
        {
            var model = await Query.FetchOneById<Model>(modelID);
            var hvm = new HatViewModel { modelID=modelID } ;
            ViewBag.Model = model;
            return View(hvm);
        }
    }
}
