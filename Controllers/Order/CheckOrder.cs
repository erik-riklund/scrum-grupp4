using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Reflection;
using iText.Layout.Borders;


namespace App.Controllers
{ 
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> ListOrders()
        {
          var allOrders = await Query.FetchAll<Entities.Order>();


            return View(allOrders.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> ApproveOrder(string orderId)
        {
            var order = await DB.Find<App.Entities.Order>().OneAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.IsApproved = true;
            order.Status = "Confirmed";
            await order.SaveAsync();

            var allOrders = await Query.FetchAll<Entities.Order>();


            
            return View("ListOrders", allOrders.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> DeclineOrder(string orderId)
        {
            var order = await DB.Find<App.Entities.Order>().OneAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

           
            order.Status = "Declined";
            await order.SaveAsync();


            var allOrders = await Query.FetchAll<Entities.Order>();



            return View("ListOrders", allOrders.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> InProgress(string orderId)
        {
            var order = await DB.Find<App.Entities.Order>().OneAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }


            order.Status = "InProgress";
            await order.SaveAsync();


            var allOrders = await Query.FetchAll<Entities.Order>();



            return View("ListOrders", allOrders.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> ReadyForDelivery(string orderId)
        {
            var order = await DB.Find<App.Entities.Order>().OneAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }


            order.Status = "ReadyForDelivery";
            await order.SaveAsync();


            var allOrders = await Query.FetchAll<Entities.Order>();



            return View("ListOrders", allOrders.ToList());
        }

    }
}

