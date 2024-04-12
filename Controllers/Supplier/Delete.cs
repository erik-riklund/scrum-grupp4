using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;

namespace App.Controllers
{
    public partial class SupplierController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> DeleteSupplier(string supplierID)
        {
            var supplier = await Query.FetchOneById<Supplier>(supplierID);

            if (supplier != null)
            {
                await supplier.DeleteAsync();

                return RedirectToAction("HandleSupplier", "supplier");
            }

            return NotFound();
        }
    }
}
