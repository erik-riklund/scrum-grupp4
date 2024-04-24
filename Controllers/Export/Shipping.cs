using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Models;
using MongoDB.Entities;
using App.Handlers;

namespace App.Controllers
{
  public partial class ExportController : Controller
  {
    [HttpGet, Guardian(Roles = "Admin")]
    public async Task<IActionResult> Shipping(string orderId)
    {
      try
      {
        ViewBag.OrderNumber = orderId;

        if (await Query.FetchOneById<Entities.Order>(orderId) is not Entities.Order order)
        {
          ModelState.AddModelError("", "The requested order does not exist.");
        }
      }

      catch (Exception)
      {
        ModelState.AddModelError("", "The requested order could not be loaded.");
      }

      return View();
    }

    [HttpPost, Guardian(Roles = "Admin")]
    public async Task<IActionResult> Shipping(PrintOrderViewModel model)
    {
      try
      {
        ViewBag.OrderNumber = model.OrderNumber;

        if (!ModelState.IsValid)
        {
          foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
          {
            ModelState.AddModelError(string.Empty, error.ErrorMessage);
          }

          return View(model);
        }

        var order = await Query.FetchOne<Entities.Order>(
           order => order.ID == model.OrderNumber
        );

        if (order != null)
        {
          var customer = await Query.FetchOneById<User>(order.CustomerID);

          if (customer == null)
          {
            ModelState.AddModelError(string.Empty, "Failed to load the customer data, try again.");

            return View(model);
          }


          var shipping = new Shipping
          {
            PackageWeight = model.PackageWeight,
            PackageContent = model.PackageContent,
            PackageQuantity = model.PackageQuantity,
            PackageSize = model.PackageSize,
            ShippingCompany = model.ShippingCompany,
            Payment = model.Payment,

          };
          var content = ShippingPdfContent.PdfContent(order, customer, shipping);
          shipping.PDF = PdfHandler.HtmlToPdf(content);
          await shipping.SaveAsync();
          await order.SaveAsync();
          await order.Shippings.AddAsync(shipping);

          return RedirectToAction("Index", "Home");
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return View(model);
    }
  }
}