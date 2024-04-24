using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using App.Handlers;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    [HttpPost]
    public async Task<IActionResult> OrderMaterial(string materialID)
    {
      try
      {
        var material = await Query.FetchOneById<Material>(materialID);

        if (material != null)
        {
          var supplier = await Query.FetchOneById<Supplier>(material.SupplierID);

          if (supplier != null)
          {
            var omvm = new OrderMaterialViewModel
            {
              Material = material,
              Supplier = supplier,
            };

            return View(omvm);
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("OrderMaterial", "Order");
    }

    [HttpPost]
    public async Task<IActionResult> PlaceMaterialOrder(string materialID, string supplierID, double amountOrdered)
    {
      try
      {
        var material = await Query.FetchOneById<Material>(materialID);
        var supplier = await Query.FetchOneById<Supplier>(supplierID);

        if (material != null && supplier != null)
        {
          ViewBag.Material = material;
          ViewBag.Supplier = supplier;
          ViewBag.Amount = amountOrdered;

          return View();
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("OrderMaterial", "Order");
    }
    [HttpPost]
    public async Task<IActionResult> OrderedPdfMaterial(string supplierID, string materialID, double amount)
    {
      try
      {
        DateTime orderDate = DateTime.Now;
        var content = await OrderMaterialPdfContent.PdfContent(supplierID, materialID, amount, orderDate);
        var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));

        return new FileStreamResult(stream, "application/pdf");
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("OrderMaterial", "Order");
    }
  }
}