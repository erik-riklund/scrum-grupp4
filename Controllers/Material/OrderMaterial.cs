using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver.Linq;
using App.Handlers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static MongoDB.Driver.WriteConcern;

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

        [HttpPost]
        public async Task<IActionResult> PlaceMaterialOrder(string materialID, string supplierID, double amountOrdered)
        {
            var material = await Query.FetchOneById<Material>(materialID);
            var supplier = await Query.FetchOneById<Supplier>(supplierID);
            double amount = amountOrdered;

            ViewBag.Material = material;
            ViewBag.Supplier = supplier;
            ViewBag.Amount = amount;
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrderedPdfMaterial(string supplierID, string materialID, Double amount)
        {
            DateTime orderDate = DateTime.Now;
            var content = await OrderMaterialPdfContent.PdfContent(supplierID, materialID, amount, orderDate);
            var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
            return new FileStreamResult(stream, "application/pdf");
        }
        
    }
}