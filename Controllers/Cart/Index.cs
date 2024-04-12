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
            var shoppingCarts = await Query.FetchAll<Cart>();
            var shoppingCart = shoppingCarts.Where(c => c.UserID == user.ID).FirstOrDefault();
            var lista = new CartViewModel();
            if(shoppingCart != null)
            {
                
                foreach(var hat in shoppingCart.Hats)
                {
                    
                    //var cartViewModel = new CartViewModel { hat = hat, model = hat.Model };
                    lista.hats.Add(hat);
                }
            }
            lista.cart = shoppingCart;

            return View(lista); 
        }
    }
}
