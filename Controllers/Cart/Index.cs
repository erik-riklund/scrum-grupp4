using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
    public partial class CartController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await session.GetUserAsync(); 
            var shoppingCart = user.ShoppingCart;
            var lista = new List<CartViewModel>();
            if(shoppingCart != null)
            {
                foreach(var hat in shoppingCart.Hats)
                {
                    var model = await Query.FetchOneById<Model>(hat.ModelID);
                    var cartViewModel = new CartViewModel { hat = hat, model = model };
                    lista.Add(cartViewModel);
                }
            }
            ViewBag.Cart = shoppingCart;

            return View(lista); 
        }
    }
}
