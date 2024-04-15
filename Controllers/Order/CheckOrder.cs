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
        public async Task<IActionResult> ListOrders()
        {
            // Hämta alla ogodkända ordrar
            var unapprovedOrders = await DB.Queryable<App.Entities.Order>()
                                            .Where(o => !o.IsApproved)
                                            .ToListAsync();

            // Hämta alla godkända ordrar
            var approvedOrders = await DB.Queryable<App.Entities.Order>()
                                            .Where(o => o.IsApproved)
                                            .ToListAsync();

            // Skapa en instans av ApproveOrderViewModel och skicka med listorna till vyn
            var viewModel = new ApproveOrderViewModel
            {
                unapprovedOrders = unapprovedOrders,
                approvedOrders = approvedOrders
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrder(string orderId)
        {
            var order = await DB.Find<App.Entities.Order>().OneAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Markera ordern som godkänd
            order.IsApproved = true;
            await order.SaveAsync();

            // Hämta alla ogodkända ordrar
            var unapprovedOrders = await DB.Queryable<App.Entities.Order>()
                                            .Where(o => !o.IsApproved)
                                            .ToListAsync();

            // Hämta alla godkända ordrar
            var approvedOrders = await DB.Queryable<App.Entities.Order>()
                                            .Where(o => o.IsApproved)
                                            .ToListAsync();

            // Skapa en instans av ApproveOrderViewModel och skicka med listorna till vyn
            var viewModel = new ApproveOrderViewModel
            {
                unapprovedOrders = unapprovedOrders,
                approvedOrders = approvedOrders
            };

            // Återvänd till samma vy med den uppdaterade datan
            return View("ListOrders", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeclineOrder(string orderId)
        {
            var order = await DB.Find<App.Entities.Order>().OneAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            // Ta bort ordern från databasen
            await order.DeleteAsync();

            // Hämta alla ogodkända ordrar
            var unapprovedOrders = await DB.Queryable<App.Entities.Order>()
                                            .Where(o => !o.IsApproved)
                                            .ToListAsync();

            // Hämta alla godkända ordrar
            var approvedOrders = await DB.Queryable<App.Entities.Order>()
                                            .Where(o => o.IsApproved)
                                            .ToListAsync();

            // Skapa en instans av ApproveOrderViewModel och skicka med listorna till vyn
            var viewModel = new ApproveOrderViewModel
            {
                unapprovedOrders = unapprovedOrders,
                approvedOrders = approvedOrders
            };

            // Återvänd till samma vy med den uppdaterade datan
            return View("ListOrders", viewModel);
        }

    }
}

