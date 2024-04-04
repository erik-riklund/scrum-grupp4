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
	}
}