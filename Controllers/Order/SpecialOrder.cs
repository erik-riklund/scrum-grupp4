using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using App.Handlers;

namespace App.Controllers
{
  public partial class OrderController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> SpecialOrderForm()
    {
      ViewBag.Material = await Query.FetchAll<Material>();

      return View(new SpecialOrderViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> SpecialOrderForm(SpecialOrderViewModel sov, List<string> selectedMaterials)
    {
      if (ModelState.IsValid)
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

        if (sov.Picture != null)
        {
          var imagehandler = new ImageHandler();

          var path = imagehandler.GetPath(sov.Picture, model.ID);
          await imagehandler.UploadImage(sov.Picture, path);
          
          model.ImagePath = path;
        }

        await model.SaveAsync();
        // await model.Hats.AddAsync(hat);

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

        hat.Model = model;
        await hat.SaveAsync();

        var order = new Entities.Order
        {
          IsApproved = false,
          Status = "SpecialPending"

        };
        
        await order.SaveAsync();
        await order.Hats.AddAsync(hat);

        return RedirectToAction("ConfirmSpecial", "Order", new { id = order.ID });
      }
      else
      {
        // felhantering vid ogiltigt formulär..?
      }

      var materials = await Query.FetchAll<Material>();
      ViewBag.Material = materials;

      return View(sov);
    }
  }
}
