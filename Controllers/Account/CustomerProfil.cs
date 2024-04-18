using App.Entities;
using App.Handlers;
using App.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Entities;
using App.Controllers;
namespace App.Controllers.Account
{
    [Guardian]
    public partial class AccountController(ISessionManager session) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> CustomerProfil()
        {
            var customer = await session.GetUserAsync();
            if (customer == null) 
            
            {
             return RedirectToAction("Index", "Home");  
            }
            var getOrder = await Query.FetchMany<Entities.Order>(order => order.CustomerID.Equals(customer.ID));
            List<Entities.Order> orderList = new List<Entities.Order>();
            foreach (var order in getOrder) 
            {
                orderList.Add((Entities.Order)order);
            }
            orderList.OrderByDescending(order => order.OrderDate);
            ViewBag.OrderLists=orderList;
            ViewBag.Customers = customer;
            return View();
        }

        //[HttpPost]

        //public async Task<IActionResult> PrintOrder(string orderId) 
        //{

        //    var content = await OrderPdfContent.PrintOneOrderHistory(orderId);

        //    var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
        //    return new FileStreamResult(stream, "application/pdf");
        //}
        [HttpPost]
        public async Task<IActionResult> PrintOrder(string orderId)
        {
            try
            {
                var getOrder = await Query.FetchOne<Entities.Order>(order => order.ID.Equals(orderId));
                var customer = await Query.FetchOne<Entities.User>(user => user.ID.Equals(getOrder.CustomerID));
                //var hatModelId = getOrder.Hats.First().Model.ID;
                //var hatModel = await Query.FetchOne<Entities.Model>(model => model.ID == hatModelId);
                var hatModel = await Query.FetchOne<App.Entities.Model>(model => model.ID.Equals(getOrder.Hats.FirstOrDefault().Model.ID));
                //var hatModel = await Query.FetchOneById<App.Entities.Model>("661ceb2a034ccebe68c32a62");

                var imageUrl = hatModel.ImagePath;
                //var imageUrl = Url.Action("GetOrderHistoryImage", "Order", new { orderId = getOrder.ID });
                var content = await OrderPdfContent.OneHistoryPdfContent(getOrder, customer, imageUrl);

                //var content = await OrderPdfContent.OneHistoryPdfContent(getOrder,customer);
                var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
                return new FileStreamResult(stream, "application/pdf");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while generating the PDF");
                // Om något går fel, returnera en vy med felmeddelandet
                return View();
            }
        }

    }

}
