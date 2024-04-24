using App.Handlers;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers.order
{
  public partial class OrderController : Controller
  {
    [HttpGet]
    public IActionResult OrderHistory()
    {
      var viewModel = new OrderHistoryViewModel
      {
        DateFrom = DateTime.Today,
        DateTo = DateTime.Today
      };

      return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> OrderHistory(OrderHistoryViewModel model)
    {
      try
      {
        var getOrder = await Query.FetchMany<Entities.Order>(order => order.OrderDate >= model.DateFrom && order.OrderDate <= model.DateTo);

        if (model.PaymentStatus)
        {
          getOrder = await Query.FetchMany<Entities.Order>(order => order.PayStatus == true);
        }
        else if (!model.PaymentStatus && !model.ChooseDate)
        {
          getOrder = await Query.FetchMany<Entities.Order>(order => order.PayStatus == false);
        }
        var getCustomer = await Query.FetchMany<Entities.User>(user => user.ID != null);
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
      }
      catch (Exception)
      {
        ModelState.AddModelError(string.Empty, "An error occurred while fetching the order history");
      }

      return View(model);
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
    public async Task<IActionResult> PrintOneOrderHistory(string orderId)
    {
      try
      {
        var getOrder = await Query.FetchOne<Entities.Order>(order => order.ID.Equals(orderId));

        if (getOrder != null)
        {
          var customer = await Query.FetchOne<Entities.User>(user => user.ID.Equals(getOrder.CustomerID));
          var hatModel = await Query.FetchOne<Entities.Model>(model => model.ID.Equals(getOrder.Hats.First().Model.ID));

          if (hatModel != null && customer != null)
          {
            var imageUrl = hatModel.ImagePath;
            var content = OrderPdfContent.OneHistoryPdfContent(getOrder, customer, imageUrl);

            var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
            return new FileStreamResult(stream, "application/pdf");
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("OrderHistory", "Order");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePaymentStatus(string orderId)
    {
      try
      {
        var getOrder = await Query.FetchOneById<Entities.Order>(orderId);

        if (getOrder != null)
        {
          if (getOrder.PayStatus)
          {
            getOrder.PayStatus = false;
          }
          else
          {
            getOrder.PayStatus = true;
          }

          await getOrder.SaveAsync();
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("OrderHistory", "Order");
    }

    [HttpPost]
    public async Task<IActionResult> PrintInvoice(string orderId)
    {
      try
      {
        var getOrder = await Query.FetchOneById<Entities.Order>(orderId);

        if (getOrder != null)
        {
          var customerId = getOrder.CustomerID;
          var customer = await Query.FetchOneById<Entities.User>(customerId);

          if (customer != null)
          {
            var content = InvoiceContentPdfcs.OneHistoryPdfContent(getOrder, customer);
            var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
            return new FileStreamResult(stream, "application/pdf");
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("OrderHistory", "Order");
    }
  }
}
