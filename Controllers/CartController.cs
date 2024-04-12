using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public partial class CartController(SessionManager session) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
