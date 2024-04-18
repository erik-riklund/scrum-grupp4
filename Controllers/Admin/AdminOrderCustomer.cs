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
    public partial class AdminController : Controller
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

        [HttpGet]
        public async Task<IActionResult> AdminSpecialOrderCustomer()
        {
            var cursor = await DB.Find<Material>().ExecuteCursorAsync();
            ViewBag.Material = await cursor.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminSpecialCustomerInfo(SpecialOrderViewModel specialOrderViewModel)
        {
            ViewBag.Description = specialOrderViewModel.Description;
            ViewBag.Size = specialOrderViewModel.Size;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceSpecialOrderForCustomer(OrderCustomerViewModel orderCustomerViewModel, List<string> selectedMaterials)
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
                IsApproved = true,
                Status = "Confirmed",
                OrderSum = orderCustomerViewModel.Price
            };

            var model = new Model
            {
                ModelName = "Specialhat",
                Description = orderCustomerViewModel.Description,
            };

            await model.SaveAsync();

            if (selectedMaterials != null && selectedMaterials.Count > 0)
            {
                foreach (var materialID in selectedMaterials)
                {
                    var material = await Query.FetchOneById<Material>(materialID);

                    if (material != null)
                    {
                        await model.Materials.AddAsync(material);
                        await material.Models.AddAsync(model);
                    }
                }
            }

            var hat = new Hat
            {
                Model = model,
                Price = orderCustomerViewModel.Price,
                Size = orderCustomerViewModel.Size,
                Description = model.Description
            };

            await hat.SaveAsync();
            await newOrder.SaveAsync();
            await newOrder.Hats.AddAsync(hat);

            ViewBag.TotalSum = newOrder.OrderSum;

            return View(newOrder);
        }
    }
}