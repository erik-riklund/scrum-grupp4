using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using App.Entities;
using iText.Layout.Borders;

namespace App.Controllers
{
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> HandleOrder(string orderId)
        {
            var order = await Query.FetchOneById<App.Entities.Order>(orderId);

            if (order == null)
            {
                return NotFound();
            }
            var viewModel = new HandleOrderViewModel
            {
                OrderId = order.ID,
                Status = order.Status,
                OrderDate = order.OrderDate,
                EstimatedDelivery = order.EstimatedDeliveryDate,
                Hats = order.Hats.ToList(),
                orderSum = order.OrderSum,
               
            };
            var customer = await Query.FetchOneById<User>(order.CustomerID);
            viewModel.customer = new UserViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                StreetAdress = customer.Address.StreetAddress,
                ZipCode = customer.Address.ZipCode,
                City = customer.Address.City,
                Country = customer.Address.Country,
                id = customer.ID
            };


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HandleOrder(HandleOrderViewModel viewModel)
        {
            var order = await Query.FetchOneById<App.Entities.Order>(viewModel.OrderId);
            order.Status = viewModel.Status;
            order.EstimatedDeliveryDate = viewModel.EstimatedDelivery;

            await order.SaveAsync();

            var allOrders = await Query.FetchAll<Entities.Order>();


            return View("ListOrders", allOrders.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromOrder(string orderId, string hatId)
        {
            var order = await Query.FetchOneById<App.Entities.Order>(orderId);
            var hat = await Query.FetchOneById<App.Entities.Hat>(hatId);

            await order.Hats.RemoveAsync(hat);
            double sum = 0;
            if (order.Hats.Count() > 0 && order.Hats != null)
            {
                foreach (Hat Hatt in order.Hats)
                {
                    sum += Hatt.Price;
                }
            }
            else { sum = 0; }
            order.OrderSum = sum;
            await order.SaveAsync();

            var viewModel = new HandleOrderViewModel
            {
                OrderId = order.ID,
                Status = order.Status,
                OrderDate = order.OrderDate,
                EstimatedDelivery = order.EstimatedDeliveryDate,
                Hats = order.Hats.ToList(),
                orderSum = order.OrderSum,
                

            };
            var customer = await Query.FetchOneById<User>(order.CustomerID);
            viewModel.customer = new UserViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                StreetAdress = customer.Address.StreetAddress,
                ZipCode = customer.Address.ZipCode,
                City = customer.Address.City,
                Country = customer.Address.Country,
                id = customer.ID
            };

            return View("HandleOrder", viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> EditOrderHat(string orderId, string hatId)
        {
            
            var hat = await Query.FetchOneById<App.Entities.Hat>(hatId);

            var viewModel = new EditOrderHatViewModel
            {
                
                OrderId = orderId,
                Size = hat.Size,
                HatId = hatId,
                HatDescription = hat.Description,
                imagePath = hat.Model.ImagePath,
                Price = hat.Price

            };
            if (hat.Model.Materials != null)
            {
                
                viewModel.Materials = hat.Model.Materials.ToList();

            }
            

             

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrderHat(EditOrderHatViewModel eoh)
        {
            var hat = await Query.FetchOneById<App.Entities.Hat>(eoh.HatId);
            hat.Price = eoh.Price;
            
            var model = await Query.FetchOneById<App.Entities.Model>(hat.Model.ID);
            model.ProductCode = eoh.ProductCode;
            await model.SaveAsync();
            await hat.SaveAsync();
            var order = await Query.FetchOneById<App.Entities.Order>(eoh.OrderId);
            double sum = 0;
            foreach (var item in order.Hats)
            {
                sum += item.Price;
            }
            order.OrderSum = sum;
            await order.SaveAsync();


            return RedirectToAction("HandleOrder", new { orderId = eoh.OrderId });
        }

        }
}
