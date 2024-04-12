using App.Interfaces;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public partial class CartController(ISessionManager session) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
