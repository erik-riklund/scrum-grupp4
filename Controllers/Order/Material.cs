using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace App.Controllers
{
    public partial class OrderController : Controller
    {
        public async Task<IActionResult> OrderMaterial()
        {
            var orderMaterialViewModel = new OrderMaterialViewModel();

            var materialCursor = await Query.FetchAll<Material>();
            var materialList = materialCursor.ToList();
            ViewBag.CurrentMaterials = materialList;


            var materialSupplier = new Dictionary<Material, Supplier>();


            foreach (var material in materialList)
            {
                var supplier = await GetSupplierForMaterial(material.SupplierID);
                materialSupplier[material] = supplier;
            }

            ViewBag.MaterialSupplierMap = materialSupplier;

            return View(orderMaterialViewModel);
        }

        private async Task<Supplier> GetSupplierForMaterial(string materialId)
        {

            var material = await Query.FetchOneById<Supplier>(materialId);
            if (material != null)
            {
                return material;
            }
            return null;

        }

    }

}



//    public partial class OrderController : Controller
//    {
//        public async Task<IActionResult> OrderMaterial()
//        {
//            var orderMaterialViewModel = new OrderMaterialViewModel();

//            var materialList = await Query.FetchAll<Material>();
//            ViewBag.CurrentMaterials = materialList.Select(m => new SelectListItem
//            {
//                Value = m.ID,
//                Text = m.Name,
//                SupplierID = m.SupplierID // Lägg till SupplierID här
//            }).ToList();

//            return View(orderMaterialViewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> GetSupplier([FromBody] string materialID)
//        {
//            var material = await Query<Material>.Where(m => m.ID == materialID).FirstOrDefaultAsync();
//            if (material != null)
//            {
//                var supplier = await Query<Supplier>.Where(s => s.ID == material.SupplierID).FirstOrDefaultAsync();
//                if (supplier != null)
//                {
//                    return Json(new
//                    {
//                        supplierName = supplier.Name,
//                        address = supplier.Address,
//                        telephoneNumber = supplier.TelephoneNumber,
//                        email = supplier.Email
//                    });
//                }
//            }
//            return Json(null);
//        }
//    }
//}
