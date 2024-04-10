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
      OrderViewModel ovm = new OrderViewModel();
      List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat").Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
      ViewBag.Models = modeller;
      return View(ovm);
    }

    [HttpPost]
    public async Task<IActionResult> OrderForm(OrderViewModel orderViewModel)
    {
      if (ModelState.IsValid)
      {

        var hat = new App.Entities.Hat
        {
          Size = orderViewModel.Size,
          Description = orderViewModel.Description,
          ModelID = orderViewModel.ModelID

        };

        await hat.SaveAsync();
        var model = await Query.FetchOneById<Model>(orderViewModel.ModelID);

        if (model != null)
        {
          await model.Hats.AddAsync(hat);
          var order = new App.Entities.Order
          {
            IsApproved = false,
            Status = "Pending"
          };
          await order.SaveAsync();
          await order.Hats.AddAsync(hat);
        }

        return RedirectToAction("Index", "Home");
      }
      var models = await Query.FetchAll<Model>();
      List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat").Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
      ViewBag.Models = modeller;
      return View(orderViewModel);
    }
  }
}
