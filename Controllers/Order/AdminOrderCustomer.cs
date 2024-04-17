using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Reflection;

namespace App.Controllers
{ 
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> OrderCustomer()
        {
            return View();
        }
    }
}