using App.Entities;
using App.Handlers;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{

    public partial class SupplierController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddNewSupplier(SupplierViewModel supplierViewModel)
        {
            if (supplierViewModel.SupplierName != null)
            {

                var address = new Address
                {
                    StreetAddress = supplierViewModel.StreetAddress,
                    ZipCode = supplierViewModel.ZipCode,
                    City = supplierViewModel.City,
                    Country = supplierViewModel.Country
                };

                var supplier = new Supplier
                {
                    Name = supplierViewModel.SupplierName,
                    Email = supplierViewModel.Email,
                    TelephoneNumber = supplierViewModel.TelephoneNumber,
                    Address = address
                };


                await supplier.SaveAsync();

                return RedirectToAction("HandleSupplier", "Supplier");
            }
            else 
            {
                 string script = "<script>alert('You must enter a supplier name!'); window.location.href='/Model/HandleModels';</script>";

                  return Content(script, "text/html");
             }
        }
    }
 }
   