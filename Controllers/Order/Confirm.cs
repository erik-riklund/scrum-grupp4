using App.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Confirm(string id)
        {
            var confirmedOrder = await Query.FetchOneById<Order>(id);

            return View(confirmedOrder);
        }
    }
}