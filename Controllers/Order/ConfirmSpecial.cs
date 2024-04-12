using App.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public partial class OrderController : Controller
  {
    public async Task<IActionResult> ConfirmSpecial(string id)
    {
      var confirmedOrder = await Query.FetchOneById<Order>(id);

      return View(confirmedOrder);
    }
  }
}