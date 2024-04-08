using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
	public class ModelController : Controller
	{
		public async Task<IActionResult> HandleModels()
		{
			var cursor = await DB.Find<Model>().ExecuteCursorAsync();
			ViewBag.CurrentModels = await cursor.ToListAsync();

			var cursor2 = await DB.Find<Material>().ExecuteCursorAsync();
			ViewBag.CurrentMaterials = await cursor2.ToListAsync();

			return View();
		}

		[HttpPost]
        public async Task<IActionResult> AddNewModel(ModelViewModel modelViewModel, List<string> SelectedMaterials) // Ändra parameter från array till List<>
        {
            if (modelViewModel.ModelName != null)
            {
                Model model = new Model
                {
                    ModelName = modelViewModel.ModelName,
                    Description = modelViewModel.Description,
                    Picture = modelViewModel.Picture,
                    ProductCode = modelViewModel.ProductCode
                };

                if (SelectedMaterials != null && SelectedMaterials.Count > 0) // Ändra från SelectedMaterials.Length till SelectedMaterials.Count
                {
                    var chosenMaterials = new List<Material>();

                    foreach (var materialId in SelectedMaterials)
                    {
                        var material = await Query.FetchOneById<Material>(materialId);

                        if (material != null)
                        {
                            chosenMaterials.Add(material);
                        }
                    }

                    await model.SaveAsync();

                    foreach (var material in chosenMaterials)
                    {
                        await model.Materials.AddAsync(material);
                    }
                }

                return RedirectToAction("HandleModels", "Model");
            }
            else
            {
                string script = "<script>alert('You must enter a model name!'); window.location.href='/Model/HandleModels';</script>";

                return Content(script, "text/html");
            }
        }

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
					Picture = model.Picture,
					ProductCode = model.ProductCode
				};

				ViewBag.ModelID = model.ID;
				return View(modelViewModel);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<IActionResult> SaveEdit(string modelID, ModelViewModel modelViewModel)
		{
			var model = await Query.FetchOneById<Model>(modelID);

			if (model != null)
			{
				model.ModelName = modelViewModel.ModelName;
				model.Description = modelViewModel.Description;
				model.Picture = modelViewModel.Picture;
				model.ProductCode = modelViewModel.ProductCode;
				await model.SaveAsync();

				return RedirectToAction("HandleModels", "Model");
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteModel(string modelID)
		{
			var model = await Query.FetchOneById<Model>(modelID);

			if (model != null)
			{
				await model.DeleteAsync();

				return RedirectToAction("HandleModels", "Model");
			}

			return NotFound();
		}
	}
}