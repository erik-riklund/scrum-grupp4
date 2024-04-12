using App.Entities;
using App.Handlers;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Text;

namespace App.Controllers.order
{
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            var viewModel = new OrderHistoryViewModel
            {
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]

        public async Task<IActionResult> orderHistory (OrderHistoryViewModel model)
        {

            var getOrder = await Query.FetchMany<Entities.Order>(order => order.OrderDate >= model.DateFrom && order.OrderDate <= model.DateTo);

            List<Entities.Order> orderList = new List<Entities.Order>();
            foreach (var order in getOrder)
            {
                orderList.Add((Entities.Order)order);
            }
            ViewBag.CurrentOrder = orderList;
            ViewBag.OrderDateFrom = model.DateFrom;
            ViewBag.OrderDateTo = model.DateTo;
            return View(model);

        }


        [HttpPost]

        public async Task<IActionResult> orderHistoryPdf(OrderHistoryViewModel model)
        {
            DateTime dateFrom = model.DateFrom;
            DateTime dateTo = model.DateTo;
            var getOrder = await Query.FetchMany<Entities.Order>(order => order.OrderDate >= dateFrom && order.OrderDate <= dateTo);

            List<Entities.Order> orderList = new List<Entities.Order>();
            foreach (var order in getOrder)
            {
                orderList.Add((Entities.Order)order);
            }
            
            var content = await OrderHistoryPdf.PdfContent(orderList, dateTo, dateFrom);
            var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
            return new FileStreamResult (stream,"application/pdf");
        }
    }
}
