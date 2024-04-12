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

        //[HttpPost]

        //public async Task<IActionResult> orderHistory()
        //{

        //}
    }
}
