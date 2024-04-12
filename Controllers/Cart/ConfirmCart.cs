using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Entities;
using MongoDB.Entities;
namespace App.Controllers
{
    public partial class CartController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> ConfirmCart(List<CartViewModel> hats)
        {
            var order= new Entities.Order();
            await order.SaveAsync();
            order.OrderSum = 0;
            foreach (var item in hats)
            {
                await order.Hats.AddAsync(item.hat);
                order.OrderSum += item.hat.Price;
            }
            var customer = await session.GetUserAsync();
            order.CustomerID = customer.ID;
            DateTime today = DateTime.Today;
            DateTime futureDate = today.AddDays(14);
            order.EstimatedDeliveryDate = futureDate;
            order.Status = "Pending";
            await order.SaveAsync();


            return RedirectToAction("Confirm", "Order", new {id=order.ID});
        }
    }
}
