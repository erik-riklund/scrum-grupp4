using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
    public class GalleryController : Controller
    {

        public IActionResult Gallery()
        {
            return View();
        }
    }
}


