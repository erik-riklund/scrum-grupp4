using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

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
