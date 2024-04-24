using Microsoft.AspNetCore.Mvc;
using App.Entities;
using App.Models;
using App.Handlers;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class CartController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> Edit(string hatID)
    {
      if (hatID != null)
      {
        try
        {
          var hat = await Query.FetchOneById<Hat>(hatID);

          if (hat != null)
          {
            var hatView = new EditHatViewModel
            {
              Id = hat.ID,
              Size = hat.Size,
              Description = hat.Description,
              Price = hat.Price
            };

            ViewBag.Model = hat.Model;

            return View(hatView);
          }
        }

        catch (Exception x)
        {
          Console.WriteLine(x.Message);
        }
      }

      return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Edit(EditHatViewModel editHat)
    {
      try
      {
        var hat = await Query.FetchOneById<Hat>(editHat.Id);
        var pricemodel = new Dictionary<Material, double>();

        if (hat != null)
        {
          foreach (var material in hat.Model.Materials)
          {
            if (material.Unit.Equals("Meter"))
            {
              pricemodel[material] = editHat.Size;
            }
            else
            {
              pricemodel[material] = 1;
            }
          }

          var pricehandler = new PriceHandler();
          var price = pricehandler.GetHatPrice(pricemodel);

          hat.Size = editHat.Size;
          hat.Description = editHat.Description;
          hat.Price = price;

          await hat.SaveAsync();
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("Index");
    }
  }

}
