using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Entities;
using System.Reflection;
using System.Runtime.InteropServices;

namespace App.Controllers
{
    public class OrderController : Controller
    {
        public async Task<IActionResult> OrderForm()
        {
            var models = await Query.FetchAll<Model>();
            OrderViewModel ovm = new OrderViewModel();
            List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat").Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
            ViewBag.Models = modeller;
            return View(ovm);
        }


        [HttpPost]
        public async Task<IActionResult> OrderForm(OrderViewModel orderViewModel)
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
            var models = await Query.FetchAll<Model>();
            List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat").Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
            ViewBag.Models = modeller;
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

        //[HttpPost]
        //public async Task<IActionResult> PostBild(TestViewModel tvn)
        //{
        //    var imagehandler = new Imagehandler();
        //    await imagehandler.UpploadImage(tvn.File);
        //    return RedirectToAction("TestBild", "Order");
        //}

        [HttpPost]
        public async Task<IActionResult> SpecialOrderForm(SpecialOrderViewModel sov, List<string> selectedMaterials)
        {
            if(ModelState.IsValid)
            {
                var hat = new Hat
                {
                    Size = sov.Size,
                    Description = sov.Description
                };


                await hat.SaveAsync();

                var model = new Model
                {
                    ModelName = "Specialhat",
                    Description = sov.Description

                };
                
                await model.SaveAsync();

                if (sov.Picture != null)
                {
                    var imagehandler = new Imagehandler();
                    var path = imagehandler.GetPath(sov.Picture, model.ID);
                    await imagehandler.UpploadImage(sov.Picture, path);
                    model.Picture = path;

                }

                await model.SaveAsync();
                await model.Hats.AddAsync(hat);

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
            var materials = await Query.FetchAll<Material>();
            ViewBag.Material = materials;
            return View(sov);


        }
    }


}


