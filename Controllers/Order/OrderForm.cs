using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class OrderController : Controller
  {
    public async Task<IActionResult> OrderForm()
    {
      var models = await Query.FetchAll<Model>();

      List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat")
        .Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();

      ViewBag.Models = modeller;
      return View(new OrderViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> OrderForm(OrderViewModel orderViewModel)
    {
      if (ModelState.IsValid)
      {
        var hat = new Hat
        {
          Size = orderViewModel.Size,
          Description = orderViewModel.Description,
          ModelID = orderViewModel.ModelID
        };

        await hat.SaveAsync();

        var customer = await session.GetUserAsync();
        var model = await Query.FetchOneById<Model>(orderViewModel.ModelID);

        if (customer != null && model != null)
        {
          var order = new Entities.Order { IsApproved = false, Status = "Pending" };

          await order.SaveAsync();
          await order.Hats.AddAsync(hat);
          await model.Hats.AddAsync(hat);
          await customer.Orders.AddAsync(order);

          return RedirectToAction("Confirm","Order", new { id = order.ID });
        }
      }

      // felhantering?
      return View(orderViewModel);
    }
  }
}
