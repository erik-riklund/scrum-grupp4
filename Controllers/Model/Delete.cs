using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class ModelController : Controller
  {
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