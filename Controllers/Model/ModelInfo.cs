using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Models;
using App.Handlers;

namespace App.Controllers
{
    public partial class ModelController : Controller
    {
        public async Task<IActionResult> ModelInfo(String modelID) 
        {
            var model = await Query.FetchOneById<Model>(modelID);
            var hvm = new HatViewModel { modelID=modelID } ;
            ViewBag.Model = model;
            var pricemodel = new Dictionary<Material, double>();
            foreach (var material in model.Materials)
            {
                if (material.Unit.Equals("Meter"))
                {
                    pricemodel[material] = 40;
                }
                else
                {
                    pricemodel[material] = 1;
                }

            }
            ViewBag.Price = new PriceHandler().GetHatPrice(pricemodel);
            return View(hvm);
        }
    }
}
