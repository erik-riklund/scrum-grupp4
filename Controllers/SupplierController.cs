using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
    public partial class SupplierController : Controller
    {
        public async Task<IActionResult> HandleSupplier()
        {
            var supplierList = await DB.Find<Supplier>().ExecuteCursorAsync();
            ViewBag.CurrentSuppliers = await supplierList.ToListAsync();

            var supplierList2 = await DB.Find<Material>().ExecuteCursorAsync();
            ViewBag.SupplierMaterial= supplierList2.ToListAsync();  

            return View();
        }
    }
}
