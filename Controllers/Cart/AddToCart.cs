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
                var model = await Query.FetchOneById<Model>(hvm.modelID);
                var pricemodel = new Dictionary<Material, double>();
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
                    Description = hvm.Description,
                    ModelID = model.ID
                };

                await hat.SaveAsync();
                await model.Hats.AddAsync(hat);

                var customer = await session.GetUserAsync();
                if (customer.ShoppingCart == null)
                {
                    customer.ShoppingCart = new Cart { UserID=customer.ID };
                    await customer.SaveAsync();
                    await customer.ShoppingCart.SaveAsync();
                    
                }
                
                var shoppingCarts = await Query.FetchAll<Cart>();
                var shoppingCart= shoppingCarts.Where(c => c.UserID == customer.ID).FirstOrDefault();

                //await shoppingCart.SaveAsync();
                await shoppingCart.Hats.AddAsync(hat);
                shoppingCart.UpdateTotalSum();
                await shoppingCart.SaveAsync();


            }
            return RedirectToAction("OurModels", "Model");
        }
    }
}
