using App.Handlers;
using App.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
      ViewBag.OrderLists = orderList;
      ViewBag.Customers = customer;
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> PrintOrder(string orderId)
    {
      try
      {
        var getOrder = await Query.FetchOne<Entities.Order>(order => order.ID.Equals(orderId));

        if (getOrder != null)
        {
          var customer = await Query.FetchOne<Entities.User>(user => user.ID.Equals(getOrder.CustomerID ?? string.Empty));
          var hatModel = await Query.FetchOne<Entities.Model>(model => model.ID.Equals(getOrder.Hats.First().Model.ID));

          if (customer != null && hatModel != null)
          {
            var imageUrl = hatModel.ImagePath;
            var content = OrderPdfContent.OneHistoryPdfContent(getOrder, customer, imageUrl);

            var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
            return new FileStreamResult(stream, "application/pdf");
          }
        }
      }
      catch (Exception)
      {
        ModelState.AddModelError(string.Empty, "An error occurred while generating the PDF");
      }

      return View();
    }
  }
}
