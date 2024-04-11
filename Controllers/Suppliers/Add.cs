using App.Entities;
using App.Handlers;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{

    public partial class ModelController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddNewSupplier(SupplierViewModel supplierViewModel, List<string> SelectedMaterials)
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



        }

      

                if (SelectedMaterials != null && SelectedMaterials.Count > 0)
                {
                    var chosenMaterials = new List<Material>();

                    foreach (var materialId in SelectedMaterials)
                    {
                        var material = await Query.FetchOneById<Material>(materialId);

                        if (material != null)
                        {
                            chosenMaterials.Add(material);
                        }
                    }

                    await model.SaveAsync();

                    foreach (var material in chosenMaterials)
                    {
                        await model.Materials.AddAsync(material);
                    }
                }

                if (imageFile != null)
                {
                    var imageHandler = new ImageHandler();
                    var path = imageHandler.GetPath(imageFile, model.ID);
                    await imageHandler.UploadImage(imageFile, path);
                    modelViewModel.ImagePath = imageFile.FileName;
                    model.ImagePath = path;
                }

                await model.SaveAsync();

                return RedirectToAction("HandleModels", "Model");
            }
            else
            {
                string script = "<script>alert('You must enter a model name!'); window.location.href='/Model/HandleModels';</script>";

                return Content(script, "text/html");
            }
        }
    }
}