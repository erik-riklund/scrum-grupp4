using App.Models;
using App.Entities;
using Microsoft.AspNetCore.Mvc;
using App.Handlers;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class CartController : Controller
  {

    [HttpGet]
    public IActionResult AddToCart()
    {
      return RedirectToAction("OurModels", "Model");
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(HatViewModel hvm)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var model = await Query.FetchOneById<Model>(hvm.modelID);
          var pricemodel = new Dictionary<Material, double>();

          if (model != null)
          {
            foreach (var material in model.Materials)
            {
              if (material.Unit.Equals("Meter"))
              {
                pricemodel[material] = hvm.Size;
              }
              else
              {
                pricemodel[material] = 1;
              }
            }

            var pricehandler = new PriceHandler();
            var price = pricehandler.GetHatPrice(pricemodel);

            var hat = new Hat
            {
              Price = price,
              Size = hvm.Size,
              Description = hvm.Description ?? string.Empty,
              Model = model
            };

            await hat.SaveAsync();

            var customer = await session.GetUserAsync();

            if (customer != null)
            {
              var cart = await Query.FetchOne<Cart>(cart => cart.UserID == customer.ID);

              if (cart == null)
              {
                cart = new Cart { UserID = customer.ID };
                await cart.SaveAsync();
              }

              await cart.Hats.AddAsync(hat);

              cart.UpdateTotalSum();
              await cart.SaveAsync();
            }
            return RedirectToAction("OurModels", "Model");
          }
        }

        catch (Exception x)
        {
          Console.WriteLine(x.Message);
        }
      }
      ViewBag.Message = "Please enter a correct headsize";
      return RedirectToAction("ModelInfo", "Model", new { modelID = hvm.modelID });

    }
  }
}
