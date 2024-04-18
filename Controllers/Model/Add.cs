using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;
using MongoDB.Entities;
using App.Handlers;

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

                           
                            // Hämta mängden från Request.Form baserat på material-ID och lägg till i MaterialUsed-dictionaryn
                            if (Request.Form.TryGetValue("MaterialUsed[" + materialId + "]", out var amountString))
                            {
                                if (double.TryParse(amountString, out double amount))
                                {
                                    modelViewModel.MaterialUsed[materialId] = amount;

                                    material.CurrentAmount -= amount;


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

                    
                }
                             


                // Sparar modellen
                

                // Sparar bilden om den finns
                if (imageFile != null)
                {
                    var imageHandler = new ImageHandler();
                    var path = imageHandler.GetPath(imageFile, model.ID);
                    await imageHandler.UploadImage(imageFile, path);
                    modelViewModel.ImagePath = imageFile.FileName;
                    model.ImagePath = path;
                }

                // Uppdaterar modellen med bildsökväg och sparar igen
                await model.SaveAsync();

                // Redirect till HandleModels-actionen i Model-controllern
                return RedirectToAction("HandleModels", "Model");
            }
            else
            {
                // Skapar ett JavaScript-skript för att visa ett felmeddelande om modellnamnet inte är angivet
                string script = "<script>alert('You must enter a model name!'); window.location.href='/Model/HandleModels';</script>";

                // Returnerar ett ContentResult med JavaScript-skriptet
                return Content(script, "text/html");
            }
        }
    }
}
