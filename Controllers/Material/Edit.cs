using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    [HttpPost]
    public async Task<IActionResult> EditMaterial(string materialID)
    {
      try
      {
        var cursor = await DB.Find<Supplier>().ExecuteCursorAsync();
        ViewBag.CurrentSuppliersThatAreAvailableInTheDatabase = await cursor.ToListAsync();

        var material = await Query.FetchOneById<Material>(materialID);

        if (material != null)
        {
          ViewBag.MaterialID = material.ID;

          var model = new MaterialViewModel
          {
            Name = material.Name,
            Description = material.Description,
            SupplierID = material.SupplierID,
            Price = material.Price,
            Unit = material.Unit,
            CurrentAmount = material.CurrentAmount
          };

          return View(model);
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("ManageMaterial", "Material");
    }

    [HttpPost]
    public async Task<IActionResult> SaveEdit(string materialID, MaterialViewModel materialViewModel)
    {
      try
      {
        var material = await Query.FetchOneById<Material>(materialID);

        if (material != null)
        {
          var currentSupplier = await Query.FetchOneById<Supplier>(material.SupplierID);
          var newSupplier = await Query.FetchOneById<Supplier>(materialViewModel.SupplierID);

          if (currentSupplier != null && newSupplier != null)
          {
            await currentSupplier.Materials.RemoveAsync(material);
            await newSupplier.Materials.AddAsync(material);
          }

          if (material != null)
          {
            material.Name = materialViewModel.Name;
            material.Description = materialViewModel.Description;
            material.SupplierID = materialViewModel.SupplierID;
            material.Price = materialViewModel.Price;
            material.Unit = materialViewModel.Unit;
            material.CurrentAmount = materialViewModel.CurrentAmount;

            await material.SaveAsync();
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("ManageMaterial", "Material");
    }
  }
}