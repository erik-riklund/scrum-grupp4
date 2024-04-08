﻿using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Entities;

namespace App.Controllers
{
    public class OrderController : Controller
    {
        public async Task<IActionResult> OrderForm()
        {
            var models = await Query.FetchAll<Model>();
            OrderViewModel ovm = new OrderViewModel();
            List<SelectListItem> modeller = models.Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
            ViewBag.Models = modeller;
            return View(ovm);
        }


        [HttpPost]
        public async Task<IActionResult> SendForm(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {

                var hat = new App.Entities.Hat
                {
                    Size = orderViewModel.Size,
                    Description = orderViewModel.Description,
                    ModelID = orderViewModel.ModelID

                };


                await hat.SaveAsync();
                var model = await Query.FetchOneById<Model>(orderViewModel.ModelID);
                await model.Hats.AddAsync(hat);
                var order = new App.Entities.Order
                {
                    IsApproved = false,
                    Status = "Pending"
                };
                await order.SaveAsync();
                await order.Hats.AddAsync(hat);

                return RedirectToAction("Index", "Home");
            }

            return View(orderViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SpecialOrderForm()
        {
            var material = await Query.FetchAll<Material>();
            SpecialOrderViewModel son = new SpecialOrderViewModel();
            ViewBag.Material = material;
            return View(son);
        }

        [HttpGet]
        public async Task<IActionResult> TestBild()
        {
            return View(new TestViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> PostBild(TestViewModel tvn)
        {
            var imagehandler = new Imagehandler();
            await imagehandler.UpploadImage(tvn.File);
            return RedirectToAction("TestBild", "Order");
        }

        [HttpPost]
        public async Task<IActionResult> PostSpecialOrder(SpecialOrderViewModel sov)
        {
            var hat = new Hat
            {
                Size = sov.Size
            };


            await hat.SaveAsync();

            var model = new Model
            {
                ModelName = "Specialhat",
                Description = sov.Description

            };

            if (sov.Picture != null)
            {
                var imagehandler = new Imagehandler();
                await imagehandler.UpploadImage(sov.Picture);
                model.Picture = sov.Picture.FileName;

            }

            await model.SaveAsync();
            await model.Hats.AddAsync(hat);

            if (sov.Materials != null)
            {
                foreach (var materialID in sov.Materials)
                {
                    var material = await Query.FetchOneById<Material>(materialID);
                    await model.Materials.AddAsync(material);
                    await material.Models.AddAsync(model);
                }


            }

            hat.ModelID = model.ID;
            await hat.SaveAsync();

            var order = new App.Entities.Order
            {
                IsApproved = false,
                Status = "SpecialPending"

            };
            await order.SaveAsync();
            await order.Hats.AddAsync(hat);

            return RedirectToAction("Index", "Home");

        }
    }


}

