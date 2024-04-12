using App.Models;
using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace App.Controllers
{
    public partial class CartController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Remove(String hatId)
        {
            var hat = await Query.FetchOneById<Hat>(hatId);

            if (hat == null)
            {
                return NotFound(); 
            }

            var user = await session.GetUserAsync();

            var shoppingCarts = await Query.FetchAll<Cart>();
            var shoppingCart = shoppingCarts.Where(c => c.UserID == user.ID).FirstOrDefault();

            if (shoppingCart == null)
            {
                return NotFound(); 
            }

            shoppingCart.Hats.RemoveAsync(hat);

            shoppingCart.UpdateTotalSum();

            await shoppingCart.SaveAsync();

            return RedirectToAction("Index"); 
        }
    }
}

