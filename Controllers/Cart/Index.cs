using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class CartController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> Index()
    {
      var lista = new CartViewModel();

      try
      {
        var user = await session.GetUserAsync();

        if (user != null)
        {
          var shoppingCart = await Query.FetchOne<Cart>(cart => cart.UserID == user.ID);

          if (shoppingCart == null)
          {
            shoppingCart = new Cart { UserID = user.ID };
            await shoppingCart.SaveAsync();
          }

          lista.cart = shoppingCart;
          lista.hats = shoppingCart.Hats.ToList();
        }
      }
      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return View(lista);
    }
  }
}
