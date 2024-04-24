using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using App.Entities;

namespace App.Controllers
{
  public partial class OrderController : Controller
  {
    [HttpGet]
    public async Task<IActionResult> HandleOrder(string orderId)
    {
      try
      {
        var order = await Query.FetchOneById<Entities.Order>(orderId);

        if (order != null)
        {
          var viewModel = new HandleOrderViewModel
          {
            OrderId = order.ID,
            Status = order.Status,
            OrderDate = order.OrderDate,
            EstimatedDelivery = order.EstimatedDeliveryDate,
            Hats = order.Hats.ToList(),
            orderSum = order.OrderSum,

          };

          var customer = await Query.FetchOneById<User>(order.CustomerID);

          if (customer != null)
          {
            viewModel.customer = new UserViewModel
            {
              FirstName = customer.FirstName,
              LastName = customer.LastName,
              Email = customer.Email,
              StreetAdress = customer.Address.StreetAddress,
              ZipCode = customer.Address.ZipCode,
              City = customer.Address.City,
              Country = customer.Address.Country,
              id = customer.ID
            };

            return View(viewModel);
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> HandleOrder(HandleOrderViewModel viewModel)
    {
      try
      {
        if (!string.IsNullOrEmpty(viewModel.OrderId))
        {
          var order = await Query.FetchOneById<Entities.Order>(viewModel.OrderId);

          if (order != null)
          {
            order.Status = viewModel.Status;
            order.EstimatedDeliveryDate = viewModel.EstimatedDelivery;

            await order.SaveAsync();

            var allOrders = await Query.FetchAll<Entities.Order>();

            return View("ListOrders", allOrders.ToList());
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return View("ListOrders", new List<Entities.Order>());
    }

    [HttpGet]
    public async Task<IActionResult> RemoveFromOrder(string orderId, string hatId)
    {
      try
      {
        var order = await Query.FetchOneById<Entities.Order>(orderId);
        var hat = await Query.FetchOneById<Hat>(hatId);

        if (order != null && hat != null)
        {
          double sum = 0;
          await order.Hats.RemoveAsync(hat);

          if (order.Hats?.Any() == true)
          {
            foreach (Hat Hatt in order.Hats)
            {
              sum += Hatt.Price;
            }
          }

          order.OrderSum = sum;
          await order.SaveAsync();

          var viewModel = new HandleOrderViewModel
          {
            OrderId = order.ID,
            Status = order.Status,
            OrderDate = order.OrderDate,
            EstimatedDelivery = order.EstimatedDeliveryDate,
            Hats = order.Hats?.ToList() ?? [],
            orderSum = order.OrderSum,
          };

          var customer = await Query.FetchOneById<User>(order.CustomerID);

          if (customer != null)
          {
            viewModel.customer = new UserViewModel
            {
              FirstName = customer.FirstName,
              LastName = customer.LastName,
              Email = customer.Email,
              StreetAdress = customer.Address.StreetAddress,
              ZipCode = customer.Address.ZipCode,
              City = customer.Address.City,
              Country = customer.Address.Country,
              id = customer.ID
            };

            return View("HandleOrder", viewModel);
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("ListOrders", "Order");
    }

    [HttpGet]
    public async Task<IActionResult> EditOrderHat(string orderId, string hatId)
    {
      try
      {
        var hat = await Query.FetchOneById<Hat>(hatId);

        if (hat != null)
        {
          var viewModel = new EditOrderHatViewModel
          {
            OrderId = orderId,
            Size = hat.Size,
            HatId = hatId,
            HatDescription = hat.Description,
            imagePath = hat.Model.ImagePath,
            Price = hat.Price
          };

          if (hat.Model.Materials != null)
          {
            viewModel.Materials = hat.Model.Materials.ToList();
          }

          return View(viewModel);
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("ListOrders", "Order");
    }

    [HttpPost]
    public async Task<IActionResult> EditOrderHat(EditOrderHatViewModel eoh)
    {
      try
      {
        var hat = await Query.FetchOneById<Hat>(eoh.HatId);

        if (hat != null)
        {
          hat.Price = eoh.Price;

          var model = await Query.FetchOneById<Model>(hat.Model.ID);

          if (model != null)
          {
            model.ProductCode = eoh.ProductCode;

            await model.SaveAsync();
            await hat.SaveAsync();

            double sum = 0;
            var order = await Query.FetchOneById<Entities.Order>(eoh.OrderId);

            if (order != null)
            {
              foreach (var item in order.Hats)
              {
                sum += item.Price;
              }

              order.OrderSum = sum;
              await order.SaveAsync();

              return RedirectToAction("HandleOrder", new { orderId = eoh.OrderId });
            }
          }
        }
      }

      catch (Exception x)
      {
        Console.WriteLine(x.Message);
      }

      return RedirectToAction("ListOrders", "Order");
    }

  }
}
