using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver.Linq;

namespace App.Controllers
{
    public partial class MaterialController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> OrderMaterial(string materialID)
        {
            var material = await Query.FetchOneById<Material>(materialID);
            var supplier = await Query.FetchOneById<Supplier>(material.SupplierID);

            var omvm = new OrderMaterialViewModel
            {
                Material = material,
                Supplier = supplier,
            };

            return View(omvm);
        }
    }
}