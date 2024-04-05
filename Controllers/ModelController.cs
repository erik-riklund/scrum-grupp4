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
			//ViewBag.CurrentModels = Query.FetchAll<Model>();
			var cursor = await DB.Find<Model>().ExecuteCursorAsync();
			ViewBag.CurrentModels = await cursor.ToListAsync();

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddNewModel(ModelViewModel modelViewModel)
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
	
				await model.SaveAsync();
	
				return RedirectToAction("HandleModels", "Model");
			}
			else
			{
				MessageBox.Show("You must enter a model name!", "Popup");
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