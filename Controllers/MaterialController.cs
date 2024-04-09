using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public class MaterialController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public async Task AddToInventory(Material material)
    {
      await material.SaveAsync();
    }

    //public async Task<Material> AdjustInventory(Material material, double amount)
    //{
    //	if (material.CurrentAmount + amount >= 0)
    //	{
    //		material.CurrentAmount += amount;

    //		await material.SaveAsync();
    //	}
    //else
    //{
    //	return View();
    //}
  }
}
