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
      var order = new Entities.Order();
      await order.SaveAsync();
      order.OrderSum = 0;

      foreach (var hat in shoppingCart.Hats)
      {
        await order.Hats.AddAsync(hat);
        order.OrderSum += hat.Price;
      }

      order.CustomerID = customer.ID;
      DateTime today = DateTime.Today;
      order.OrderDate = today;
      DateTime futureDate = today.AddDays(14);
      order.EstimatedDeliveryDate = futureDate;

      await order.SaveAsync();

      foreach (var orderdhat in shoppingCart.Hats)
      {
        await shoppingCart.Hats.RemoveAsync(orderdhat);
      }

      order.Status = "Confirmed";
      order.IsApproved = true;
      order.OrderSum = shoppingCart.TotalSum;

      shoppingCart.UpdateTotalSum();
      await shoppingCart.SaveAsync();
      await order.SaveAsync();

      return RedirectToAction("Confirm", "Order", new { id = order.ID });
    }
  }
}
