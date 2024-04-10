using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
    public partial class MaterialController : Controller
    {
        public async Task AddToInventory(Material material)
        {
            await material.SaveAsync();
        }
    }
}
