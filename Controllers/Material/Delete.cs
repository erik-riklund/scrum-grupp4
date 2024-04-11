using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    [HttpPost]
    public async Task<IActionResult> DeleteMaterial(string materialID)
    {
      var material = await Query.FetchOneById<Material>(materialID);

      if (material != null)
      {
        await material.DeleteAsync();

        return RedirectToAction("ManageMaterial", "Material");
      }

      return NotFound();
    }
  }
}