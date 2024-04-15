using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
namespace App.Controllers
{
    public partial class CartController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> ConfirmCart()
        {
            var customer = await session.GetUserAsync();
            var shoppingCarts = await Query.FetchAll<Cart>();
            var shoppingCart = shoppingCarts.Where(c => c.UserID == customer.ID).FirstOrDefault();
            var order= new Entities.Order();
            await order.SaveAsync();
            order.OrderSum = 0;
            bool specialHat = false;
            foreach (var hat in shoppingCart.Hats)
            {
                await order.Hats.AddAsync(hat);
                order.OrderSum += hat.Price;
                if(hat.Model.ModelName.Equals("Specialhat"))
                {
                    specialHat = true;
                }
            }
            
            order.CustomerID = customer.ID;
            DateTime today = DateTime.Today;
            DateTime futureDate = today.AddDays(14);
            order.EstimatedDeliveryDate = futureDate;
            
            await order.SaveAsync();
            shoppingCart.TotalSum = 0;
            foreach (var orderdhat in shoppingCart.Hats)
            {
                shoppingCart.Hats.RemoveAsync(orderdhat);
            }

            if(specialHat)
            {
                order.Status = "Pending";
                await order.SaveAsync();
                return RedirectToAction("ConfirmSpecial", "Order", new { id = order.ID });
            }
            order.Status = "Confirmed";
            await order.SaveAsync();
            return RedirectToAction("Confirm", "Order", new {id=order.ID});
        }
    }
}
