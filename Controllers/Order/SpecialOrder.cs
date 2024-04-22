using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using App.Handlers;

namespace App.Controllers
{
  public partial class OrderController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> SpecialOrderForm()
    {
      ViewBag.Material = await Query.FetchAll<Material>();

      return View(new SpecialOrderViewModel());
    }

    [HttpPost, Guardian]
    public async Task<IActionResult> SpecialOrderForm(SpecialOrderViewModel sov, List<string> selectedMaterials)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var hat = new Hat
          {
            Size = sov.Size,
            Description = sov.Description,
            Price = 0
          };
          await hat.SaveAsync();

          var model = new Model
          {
            ModelName = "Specialhat",
            Description = sov.Description
          };
          await model.SaveAsync();

          if (sov.Picture != null)
          {
            var imagehandler = new ImageHandler();

            var path = imagehandler.GetPath(sov.Picture, model.ID);
            await imagehandler.UploadImage(sov.Picture, path);

            model.ImagePath = path;
          }

          await model.SaveAsync();

          if (selectedMaterials != null && selectedMaterials.Count > 0)
          {
            foreach (var materialID in selectedMaterials)
            {
              var material = await Query.FetchOneById<Material>(materialID);

              if (material != null)
              {
                await model.Materials.AddAsync(material);
                await material.Models.AddAsync(model);
              }
            }
          }

          hat.Model = model;
          await hat.SaveAsync();

          if (await session.GetUserAsync() is User customer)
          {
            if (model != null)
            {
              var order = new Entities.Order
              {
                CustomerID = customer.ID,
                IsApproved = false,
                Status = "Pending"
              };

              if (await Query.FetchOneById<User>("661631d6fdc5b0a63f5d5241") is User otto)
              {
                string title = sov.Description.Length > 30 ? string.Concat(sov.Description.AsSpan(0, 30), "...") : sov.Description;

                var topic = new Topic
                {
                  Sender = customer,
                  Recipient = otto,
                  Title = $"Special order: {title}"
                };

                var message = new Message
                {
                  Sender = customer,
                  Content = sov.Description
                };

                await order.SaveAsync();
                await order.Hats.AddAsync(hat);
                await customer.Orders.AddAsync(order);

                await message.SaveAsync();
                await topic.SaveAsync();
                await topic.Messages.AddAsync(message);

                return RedirectToAction("ConfirmSpecial", "Order", new { id = order.ID });
              }
            }
          }

        }

        catch (Exception)
        {
          ModelState.AddModelError("", "There was an error processing the request, please try again.");
        }
      }

      var materials = await Query.FetchAll<Material>();
      ViewBag.Material = materials;

      return View(sov);
    }
  }
}
