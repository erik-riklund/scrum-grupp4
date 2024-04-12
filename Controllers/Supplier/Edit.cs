using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;
using App.Handlers;

namespace App.Controllers
{
    public partial class SupplierController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> EditSupplier(string supplierId)
        {
            var supplier = await Query.FetchOneById<Supplier>(supplierId);

            if (supplier != null)
            {
                var supplierViewModel = new SupplierViewModel
                {
                    SupplierName = supplier.Name,
                    Email = supplier.Email,
                    TelephoneNumber = supplier.TelephoneNumber,
                    StreetAddress = supplier.Address.StreetAddress,
                    ZipCode = supplier.Address.ZipCode,
                    City = supplier.Address.City,
                    Country = supplier.Address.Country, 
                };

                ViewBag.SupplierId = supplier.ID;

                ViewBag.CurrentSupplier = supplier;


                return View(supplierViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(string supplierId, SupplierViewModel supplierViewModel)
        {
            var supplier = await Query.FetchOneById<Supplier>(supplierId);

            if (supplier != null)
            {
                

                supplier.Name = supplierViewModel.SupplierName;
                supplier.Email = supplierViewModel.Email;
                supplier.TelephoneNumber = supplierViewModel.TelephoneNumber;
                supplier.Address.StreetAddress = supplierViewModel.StreetAddress;
                supplier.Address.ZipCode = supplierViewModel.ZipCode;
                supplier.Address.City = supplierViewModel.City;
                supplier.Address.Country = supplierViewModel.Country;

                

                await supplier.SaveAsync();


                return RedirectToAction("HandleSupplier", "Supplier");
            }

            return NotFound();
        }
    }
}