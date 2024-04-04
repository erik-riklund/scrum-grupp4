using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
	public class ModelController : Controller
	{
		public async Task<IActionResult> HandleModels()
		{
			return View();
		}
	}
}