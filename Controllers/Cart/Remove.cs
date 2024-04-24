using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class CartController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> Remove(string hatId)
    {
      try
      {
        var hat = await Query.FetchOneById<Hat>(hatId);

        if (hat != null)
        {
          var user = await session.GetUserAsync();

          if (user != null)
          {
            var shoppingCart = await Query.FetchOne<Cart>(cart => cart.UserID == user.ID);

            if (shoppingCart != null)
            {
              await shoppingCart.Hats.RemoveAsync(hat);
              await hat.DeleteAsync();

              shoppingCart.UpdateTotalSum();

              await shoppingCart.SaveAsync();
            }
          }
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
