using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;
using App.Handlers;

namespace App.Controllers
{
  public partial class ModelController : Controller
  {
    [HttpPost]
    public async Task<IActionResult> EditModel(string modelID)
    {
      var model = await Query.FetchOneById<Model>(modelID);

      if (model != null)
      {
        var modelViewModel = new ModelViewModel
        {
          ModelName = model.ModelName,
          Description = model.Description,
          ImagePath = model.ImagePath,
          ProductCode = model.ProductCode
        };

        ViewBag.ModelID = model.ID;

        ViewBag.CurrentModel = model;

        var linkedMaterialIds = model.Materials.Select(m => m.ID).ToList();

        ViewBag.LinkedMaterials = model.Materials;

        ViewBag.OtherMaterials = await DB.Find<Material>()
          .Match(m => !linkedMaterialIds.Contains(m.ID))
          .ExecuteAsync();

        return View(modelViewModel);
      }
      else
      {
        return NotFound();
      }
    }

    [HttpPost]
    public async Task<IActionResult> SaveEdit(string modelID, ModelViewModel modelViewModel, List<string> SelectedMaterials, IFormFile imageFile)
    {
      var model = await Query.FetchOneById<Model>(modelID);

      if (model != null)
      {
        var originalPath = model.ImagePath;

        model.ModelName = modelViewModel.ModelName;
        model.Description = modelViewModel.Description;
        model.ImagePath = modelViewModel.ImagePath;
        model.ProductCode = modelViewModel.ProductCode;

        await model.Materials.RemoveAsync(model.Materials);

        if (SelectedMaterials != null && SelectedMaterials.Count > 0)
        {
          var chosenMaterials = await DB.Find<Material>().Match(m => SelectedMaterials.Contains(m.ID)).ExecuteAsync();

          foreach (var material in chosenMaterials)
          {
            await model.Materials.AddAsync(material);
          }
        }

        await model.SaveAsync();

        if (imageFile != null)
        {
          var imageHandler = new ImageHandler();
          var path = imageHandler.GetPath(imageFile, model.ID);
          await imageHandler.UploadImage(imageFile, path);
          modelViewModel.ImagePath = imageFile.FileName;
          model.ImagePath = path;
          await model.SaveAsync();
        }
        else
        {
          model.ImagePath = originalPath;
          await model.SaveAsync();
        }

        return RedirectToAction("HandleModels", "Model");
      }

      return NotFound();
    }
  }
}