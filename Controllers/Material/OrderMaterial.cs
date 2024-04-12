using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Controllers
{
    public partial class MaterialController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> OrderMaterial(string materialID)
        {
            var material = await Query.FetchOneById<Material>(materialID);
            
            var supplier = await Query.FetchOne<Supplier>(supplier => supplier.Materials.Contains(material));

            var omvm = new OrderMaterialViewModel
            {
                Material = material,
                Supplier = supplier
            };

            return View(omvm);
        }
    }
}