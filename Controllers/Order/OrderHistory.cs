using App.Entities;
using App.Handlers;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers.Order
{
    public partial class OrderController : Controller
    {
        [HttpGet]
        public IActionResult OrderHistory()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> orderHistory(OrderHistoryViewModel model)
        {

            var getOrder = await Query.FetchMany<Entities.Order>(order => order.OrderDate >= model.DateFrom && order.OrderDate <=model.DateTo);
            List<Entities.Order> orderList = new List<Entities.Order>();
            foreach (var order in getOrder)
            {
                orderList.Add((Entities.Order)order);
            }
            
            

            DateTime dateTo = model.DateTo;
            DateTime dateFrom = model.DateFrom;
            var content = OrderHistoryPdf.PdfContent(orderList, dateTo, dateFrom);
            return RedirectToAction("Index", "Home");
        }
    }
}
