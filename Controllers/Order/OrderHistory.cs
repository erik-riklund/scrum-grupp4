﻿using App.Entities;
using App.Handlers;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Diagnostics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;


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

        public async Task<IActionResult> OrderHistory (OrderHistoryViewModel model)
        {
            try
            {   var getOrder = await Query.FetchMany<Entities.Order>(order => order.OrderDate >= model.DateFrom && order.OrderDate <= model.DateTo);

                if (model.PaymentStatus)
                {
                    getOrder = await Query.FetchMany<Entities.Order>(order => order.PayStatus ==true);
                }
                else if (!model.PaymentStatus && !model.ChooseDate)
                { 
                    getOrder = await Query.FetchMany<Entities.Order>(order => order.PayStatus == false);
                }
                var getCustomer = await Query.FetchMany<Entities.User>(user => user.ID !=null);
                List<Entities.Order> orderList = new List<Entities.Order>();
                List<Entities.User> userList = new List<Entities.User>();
                foreach (var order in getOrder)
                {
                    orderList.Add((Entities.Order)order);
                }
                foreach (var customer in getCustomer)
                {
                    userList.Add((Entities.User)customer);
                }
                ViewBag.CurrentCustomer = userList;
                ViewBag.CurrentOrder = orderList;
                ViewBag.OrderDateFrom = model.DateFrom;
                ViewBag.OrderDateTo = model.DateTo;
                return View(model);
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the order history");

                return View(model);
            }
            

        }


        [HttpPost]

        public async Task<IActionResult> AllOrderHistoryPdf(OrderHistoryViewModel model)
        {
            DateTime dateFrom = model.DateFrom;
            DateTime dateTo = model.DateTo;
            try 
            {
                var getOrder = await Query.FetchMany<Entities.Order>(order => order.OrderDate >= dateFrom && order.OrderDate <= dateTo);

                List<Entities.Order> orderList = new List<Entities.Order>();
                foreach (var order in getOrder)
                {
                    orderList.Add((Entities.Order)order);
                }

                var content = await OrderHistoryPdf.PdfContent(orderList, dateTo, dateFrom);
                var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
                return new FileStreamResult(stream, "application/pdf");
            } 
            catch 
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the order history");
                return View(model);
            }
         
        }

        [HttpPost]
        public async Task<IActionResult> PrintOneOrderHistory (string orderId)
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
            catch 
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the order history");
                return View();
            }
           
            
        }
        [HttpPost]
        public async Task<IActionResult> ChangePaymentStatus (string orderId) 
        {
            try 
            {
                var getOrder = await Query.FetchOneById<Entities.Order>(orderId);
                if(getOrder.PayStatus && getOrder.PayStatus !=null)
                {
                    getOrder.PayStatus = false;
                }
                else 
                {
                    getOrder.PayStatus=true;
                }
                getOrder.SaveAsync();

                return RedirectToAction("OrderHistory", "Order");
            }
            catch 
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the order history");
                return RedirectToAction("OrderHistory", "Order");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PrintInvoice (string orderId)
        {
            try
            {
                var getOrder = await Query.FetchOneById<Entities.Order>(orderId);
                var customerId = getOrder.CustomerID;
                var customer = await Query.FetchOneById<Entities.User>(customerId);

                var content = await InvoiceContentPdfcs.OneHistoryPdfContent(getOrder, customer);

                var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
                return new FileStreamResult(stream, "application/pdf");

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the order");
                return RedirectToAction("OrderHistory", "Order");
            }

        }

    }
}
