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
		public async Task<IActionResult> AddNewModel(ModelViewModel modelViewModel, List<string> SelectedMaterials, IFormFile imageFile)
		{
			if (modelViewModel.ModelName != null)
			{
				Model model = new Model
				{
					ModelName = modelViewModel.ModelName,
					Description = modelViewModel.Description,
					ProductCode = modelViewModel.ProductCode
				};

				if (SelectedMaterials != null && SelectedMaterials.Count > 0)
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

				if (imageFile != null)
				{
					var imageHandler = new ImageHandler();
					var path = imageHandler.GetPath(imageFile, model.ID);
					await imageHandler.UploadImage(imageFile, path);
					modelViewModel.ImagePath = imageFile.FileName;
					model.ImagePath = path;
				}

				await model.SaveAsync();

				return RedirectToAction("HandleModels", "Model");
			}
			else
			{
				string script = "<script>alert('You must enter a model name!'); window.location.href='/Model/HandleModels';</script>";

				return Content(script, "text/html");
			}
		}
  }
}