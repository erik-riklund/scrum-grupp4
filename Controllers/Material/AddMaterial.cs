using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MaterialController : Controller
  {
    [HttpPost]
    public async Task<IActionResult> AddNewMaterial(MaterialViewModel materialViewModel)
    {
      var materialModel = new Material
      {
        Name = materialViewModel.Name,
        Description = materialViewModel.Description,
        SupplierID = materialViewModel.SupplierID,
        Price = materialViewModel.Price,
        Unit = materialViewModel.Unit,
        CurrentAmount = materialViewModel.CurrentAmount
      };

      await materialModel.SaveAsync();

      var supplier = await Query.FetchOneById<Supplier>(materialViewModel.SupplierID);

      if (supplier != null)
      {
        await supplier.Materials.AddAsync(materialModel);
      }

      return RedirectToAction("ManageMaterial", "Material");
    }
  }
}
