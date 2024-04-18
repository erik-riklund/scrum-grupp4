using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using App.Entities;
using iText.Layout.Borders;

namespace App.Controllers
{
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> HandleOrder(string orderId)
        {
            var order = await Query.FetchOneById<App.Entities.Order>(orderId);

            if (order == null)
            {
                return NotFound();
            }
            var viewModel = new HandleOrderViewModel
            {
                OrderId = order.ID,
                Status = order.Status,
                OrderDate = order.OrderDate,
                EstimatedDelivery = order.EstimatedDeliveryDate,
                Hats = order.Hats.ToList(),
                orderSum = order.OrderSum,
               
            };
            var customer = await Query.FetchOneById<User>(order.CustomerID);
            viewModel.customer = new UserViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                StreetAdress = customer.Address.StreetAddress,
                ZipCode = customer.Address.ZipCode,
                City = customer.Address.City,
                Country = customer.Address.Country,
                id = customer.ID
            };


            return View(viewModel);
        }
    }
}
