using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;
using App.Handlers;
using static MongoDB.Driver.WriteConcern;

namespace App.Controllers
{
    public partial class ModelController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddNewModel(ModelViewModel modelViewModel, List<string> SelectedMaterials, IFormFile imageFile)
        {
            if (modelViewModel.ModelName != null)
            {
                Model model = new Model
                {
                    ModelName = modelViewModel.ModelName,
                    Description = modelViewModel.Description,
                    ProductCode = modelViewModel.ProductCode,
                    Amount = modelViewModel.Amount,
                };

                if (SelectedMaterials != null && SelectedMaterials.Count > 0)
                {
                    var chosenMaterials = new List<Material>();

                    foreach (var materialId in SelectedMaterials)
                    {
                        var material = await Query.FetchOneById<Material>(materialId);

                        if (material != null)
                        {
                            chosenMaterials.Add(material);

                           
                           if (Request.Form.TryGetValue("MaterialUsed[" + materialId + "]", out var amountString))
                            {
                                if (double.TryParse(amountString, out double amount))
                                {
                                    modelViewModel.MaterialUsed[materialId] = amount;

                                 


                                }
                            }
                                                       
                     
                        }

                     

                    }

                    await model.SaveAsync();

                    foreach (var material in chosenMaterials)
                    {
                        await model.Materials.AddAsync(material);
                    }

                    

                    var keysToRemove = modelViewModel.MaterialUsed.Where(x => x.Value == 0).Select(x => x.Key).ToList();
                    foreach (var key in keysToRemove)
                    {
                        modelViewModel.MaterialUsed.Remove(key);
                    }

                    foreach (var materialEntry in modelViewModel.MaterialUsed) 
                    {

                        var materialId = materialEntry.Key;
                        var amount = materialEntry.Value;

                        var material = await Query.FetchOneById<Material>(materialId);

                        if (material != null)
                        {
                            var totalAmount = amount * modelViewModel.Amount;

                            if (material.CurrentAmount > totalAmount)
                            {

                                material.CurrentAmount -= totalAmount;

                            }

                            else 
                            {

                                string script = "<script>alert('There is not enough material in stock!'); window.location.href='/Model/HandleModels';</script>";
                                return Content(script, "text/html");
                            }
                            await material.SaveAsync();
                        }

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
