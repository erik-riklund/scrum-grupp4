using App.Models;
using App.Entities;
using Microsoft.AspNetCore.Mvc;
using App.Handlers;
using MongoDB.Entities;
using App.Services;

namespace App.Controllers
{
    public partial class CartController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddToCart(HatViewModel hvm)
        {
            if (ModelState.IsValid)
            {
                var pricemodel = new Dictionary<Material, double>();
                foreach (var material in hvm.HatModel.Materials)
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
                var model = hvm.HatModel;

                var hat = new Hat
                {
                    Price = price,
                    Size = hvm.Size,
                    Description = hvm.Description,
                    ModelID = model.ID
                };

                await hat.SaveAsync();
                await model.Hats.AddAsync(hat);

                var customer = await session.GetUserAsync();
                if (customer.ShoppingCart == null)
                {
                    customer.ShoppingCart = new Cart();
                }
                
                
                await customer.ShoppingCart.Hats.AddAsync(hat);
                customer.ShoppingCart.UpdateTotalSum();
                await customer.ShoppingCart.SaveAsync();


            }
            return RedirectToAction("OurModels", "Model");
        }
    }
}
