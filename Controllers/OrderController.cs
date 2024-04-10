using App.Entities;
using App.Interfaces;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Entities;
using System.Diagnostics;
using MongoDB.Driver.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Runtime.InteropServices;

namespace App.Controllers
{
    [Guardian]
    public class OrderController(ISessionManager sessionManager) : Controller
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

        var customer = await sessionManager.GetUserAsync();

        await hat.SaveAsync();
        var model = await Query.FetchOneById<Model>(orderViewModel.ModelID);
        await model.Hats.AddAsync(hat);
            //var models = await Query.FetchAll<Model>();
            //OrderViewModel ovm = new OrderViewModel();
            //List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat").Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
            //ViewBag.Models = modeller;
            //return View(ovm);
        //}

        //[HttpPost]
        //public async Task<IActionResult> OrderForm(OrderViewModel orderViewModel)
        //{
            //if (ModelState.IsValid)
            //{

                //var hat = new App.Entities.Hat
                //{
                    //Size = orderViewModel.Size,
                    //Description = orderViewModel.Description,
                    //ModelID = orderViewModel.ModelID

                //};

                //await hat.SaveAsync();
                //var model = await Query.FetchOneById<Model>(orderViewModel.ModelID);
                //await model.Hats.AddAsync(hat);
                var order = new App.Entities.Order
                {
                    IsApproved = false,
                    Status = "Pending",
                  CustomerId = customer.ID
        };
        await order.SaveAsync();
        await order.Hats.AddAsync(hat);
        await customer.Orders.AddAsync(order);
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
                //return RedirectToAction("Index", "Home");
            //}
            //var models = await Query.FetchAll<Model>();
            //List<SelectListItem> modeller = models.Where(x => x.ModelName != "Specialhat").Select(x => new SelectListItem { Text = x.ModelName, Value = x.ID }).ToList();
            //ViewBag.Models = modeller;
            //return View(orderViewModel);
        }


      }

      hat.ModelID = model.ID;
      await hat.SaveAsync();

            var customer = await sessionManager.GetUserAsync();

            var order = new App.Entities.Order
            {
                IsApproved = false,
                Status = "SpecialPending",
        CustomerId = customer.ID
      };
      await order.SaveAsync();
      await order.Hats.AddAsync(hat);
      await customer.Orders.AddAsync(order);

      return RedirectToAction("Index", "Home");

    }

    [HttpGet]
    public async Task<IActionResult> PrintOrder(string orderNumber)
    {
      try
      {
        var order = await Query.FetchOneById<App.Entities.Order>(orderNumber);
        ViewBag.OrderNumber = orderNumber;

        if (order == null)
        {
          ModelState.AddModelError("", "The ordernumber does not exist");
        }

        // var customer = await Query.FetchOne<App.Entities.User>(user => user.Orders.Any(order => order.ID == orderNumber));

        //return View(); // fortsätt här ...
      }

      catch (Exception ex)
      {
        Debug.WriteLine(ex);

        ModelState.AddModelError("", "The order could not be loaded.");
      }

      return View(); // felhantering?
    }

    [HttpPost]
    public async Task<IActionResult> PrintOrder(PrintOrderViewModel model)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
          {
            ModelState.AddModelError(string.Empty, error.ErrorMessage);
          }

          return View(model);
        }

       var order = await Query.FetchOne<App.Entities.Order>(
           order => order.ID == model.OrderNumber
          );

       var customer = await Query.FetchOneById<App.Entities.User>(order.CustomerId
                );

                if (customer == null)
        {
          Debug.WriteLine($"Ingen kund, ordernr: {model.OrderNumber}");
          ModelState.AddModelError(string.Empty, "Failed to load the customer data, try again.");
          
          return View(model);
        }

                //var order = customer.Orders.Where(order => order.ID.Equals(model.OrderNumber)).First();

                if (order == null)
                {
                    Debug.WriteLine("Ingen order");
                }

                var shipping = new Shipping
                {
                    PackageWeight = model.PackageWeight,
                    PackageContent = model.PackageContent,
                    PackageQuantity = model.PackageQuantity,
                    PackageSize = model.PackageSize,
                    ShippingCompany = model.ShippingCompany,
                    Payment = model.Payment
                };

               

                await shipping.SaveAsync();
                await order.SaveAsync();

                await order.Shippings.AddAsync(shipping);
                PrintController.GeneratePDFShipping(model, order, customer);
            }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);

        // mer felhantering?
      }

      return View();
    }

        private void BarcodeGeneretor(string orderNumber)
        {
        
        }

       
    }

        //[HttpPost]
        //public async Task<IActionResult> SpecialOrderForm(SpecialOrderViewModel sov, List<string> selectedMaterials)
        //{
            //if(ModelState.IsValid)
            //{
                //var hat = new Hat
                //{
                    //Size = sov.Size,
                    //Description = sov.Description
                //};


                //await hat.SaveAsync();

                //var model = new Model
                //{
                    //ModelName = "Specialhat",
                    //Description = sov.Description

                //};
                
                //await model.SaveAsync();

                //if (sov.Picture != null)
                //{
                    //var imagehandler = new Imagehandler();
                    //var path = imagehandler.GetPath(sov.Picture, model.ID);
                    //await imagehandler.UploadImage(sov.Picture, path);
                    //model.ImagePath = path;
                //}

                //await model.SaveAsync();
                //await model.Hats.AddAsync(hat);

                //if (selectedMaterials != null && selectedMaterials.Count > 0)
                //{
                    //foreach (var materialID in selectedMaterials)
                    //{
                        //var material = await Query.FetchOneById<Material>(materialID);
                        //if (material != null)
                        //{
  //                          await model.Materials.AddAsync(material);
//                            await material.Models.AddAsync(model);
    //                    }
//
  //                  }


    //            }

      //          hat.ModelID = model.ID;
        //        await hat.SaveAsync();
//
  //              var order = new App.Entities.Order
    //            {
      //              IsApproved = false,
        //            Status = "SpecialPending"
//
  //              };
    //            await order.SaveAsync();
      //          await order.Hats.AddAsync(hat);
//
  //              return RedirectToAction("Index", "Home");
    //        }
      //      
        //    var materials = await Query.FetchAll<Material>();
          //  ViewBag.Material = materials;
            //
  //          return View(sov);
    //    }
    //}
}
