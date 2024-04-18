using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Reflection;
using App.Handlers;
using System.Diagnostics.CodeAnalysis;

namespace App.Controllers
{
    public partial class OrderController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> AdminOrderCustomer()
        {
            var models = await Query.FetchAll<Model>();
            List<Model> modelList = models.Where(m => m.ModelName != "Specialhat").ToList();
            return View(modelList);
        }

        [HttpGet]
        public async Task<IActionResult> HatOrderForCustomer(string modelID)
        {
            var model = await Query.FetchOneById<Model>(modelID);
            var hvm = new HatViewModel { modelID = modelID };
            ViewBag.Model = model;
            var pricemodel = new Dictionary<Material, double>();

            foreach (var material in model.Materials)
            {
                if (material.Unit.Equals("Meter"))
                {
                    pricemodel[material] = 40;
                }
                else
                {
                    pricemodel[material] = 1;
                }

            }
            ViewBag.Price = new PriceHandler().GetHatPrice(pricemodel);

            return View(hvm);
        }

        [HttpGet]
        public async Task<IActionResult> AdminCustomerOrderInfo(string modelID, double size)
        {
            var model = await Query.FetchOneById<Model>(modelID);
            ViewBag.SelectedModel = model;
            ViewBag.Size = size;

            var priceHandler = new PriceHandler();

            var materials = model.Materials.ToList();
            Dictionary<Material, double> dictionary = new Dictionary<Material, double>();

            foreach (var material in model.Materials)
            {
                dictionary.Add(material, size);
            }

            ViewBag.Price = priceHandler.GetHatPrice(dictionary);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrderForCustomer(OrderCustomerViewModel orderCustomerViewModel)
        {
            var newAddress = new Address
            {
                StreetAddress = orderCustomerViewModel.StreetAddress,
                ZipCode = orderCustomerViewModel.ZipCode,
                City = orderCustomerViewModel.City,
                Country = orderCustomerViewModel.Country
            };

            await newAddress.SaveAsync();

            var newUser = new User
            {
                FirstName = orderCustomerViewModel.FirstName,
                LastName = orderCustomerViewModel.LastName,
                Email = orderCustomerViewModel.Email,
                PhoneNumber = orderCustomerViewModel.PhoneNumber,
                Address = newAddress
            };

            await newUser.SaveAsync();

            var newOrder = new App.Entities.Order
            {
                CustomerID = newUser.ID,
                OrderDate = DateTime.Now,
                EstimatedDeliveryDate = DateTime.Now.AddDays(14),
                IsApproved = true,
                Status = "Confirmed",
                OrderSum = orderCustomerViewModel.Price
            };

            var selectedModel = await Query.FetchOneById<Model>(orderCustomerViewModel.ModelID);

            var hat = new Hat
            {
                Model = selectedModel,
                Price = orderCustomerViewModel.Price,
                Size = orderCustomerViewModel.Size,
                Description = ""
            };

            await hat.SaveAsync();
            await newOrder.SaveAsync();
            await newOrder.Hats.AddAsync(hat);

            ViewBag.TotalSum = newOrder.OrderSum;

            return View(newOrder);
        }
    }
}