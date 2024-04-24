using Microsoft.AspNetCore.Mvc;
using App.Entities;
using MongoDB.Entities;
using MongoDB.Driver;
using App.Models;

namespace App.Controllers.Account
{
  public partial class AccountController : Controller
  {
    [HttpGet, Guardian(Roles = "Admin")]
    public async Task<IActionResult> EditCustomer()
    {
      try
      {
        var user = await session.GetUserAsync();
        var cursor = await DB.Find<User>().ExecuteCursorAsync();
        var customers = await cursor.ToListAsync();

        var sortedCustomers = customers.OrderBy(c => c.FirstName).ToList();

        ViewBag.Customers = sortedCustomers;
        ViewBag.User = user;
      }
      catch (Exception)
      {
        ModelState.AddModelError(string.Empty, "An error occurred while fetching customers");
      }

      return View();
    }

    [HttpPost, Guardian(Roles = "Admin")]
    public async Task<IActionResult> EditCustomerInput(string customerId)
    {
      try
      {
        var customer = await Query.FetchOneById<User>(customerId);
        ViewBag.Customer = customer;
        if (customer != null)
        {
          var editCustomerInputViewModel = new EditCustomerViewModel
          {
            CustomerId = customer.ID,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            StreetAddress = customer.Address.StreetAddress,
            ZipCode = customer.Address.ZipCode,
            City = customer.Address.City,
            Country = customer.Address.Country,
          };

          return View(editCustomerInputViewModel);
        }
      }
      catch
      {
        ModelState.AddModelError(string.Empty, "An error occurred while fetching customers");
      }

      return RedirectToAction("EditCustomer", "Account");
    }

    [HttpPost, Guardian(Roles = "Admin")]
    public async Task<IActionResult> SaveCustomer(EditCustomerViewModel model)
    {
      try
      {
        if (ModelState.IsValid)
        {
          var customer = await Query.FetchOneById<User>(model.CustomerId);
          if (customer != null)
          {
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.Address.StreetAddress = model.StreetAddress;
            customer.Address.ZipCode = model.ZipCode;
            customer.Address.City = model.City;
            customer.Address.Country = model.Country;

            await customer.SaveAsync();
          }

          return RedirectToAction("EditCustomer", "Account");
        }
      }
      catch (Exception)
      {
        ModelState.AddModelError(string.Empty, "An error occurred while fetching customers");
      }

      return View(model);
    }

    [HttpPost, Guardian(Roles = "Admin")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
      try
      {
        var customer = await Query.FetchOneById<User>(customerId);
        if (customer != null)
        {
          await customer.DeleteAsync();
        }
        return RedirectToAction("EditCustomer", "Account");

      }
      catch
      {
        ModelState.AddModelError(string.Empty, "An error occurred while fetching customers");
        return RedirectToAction("EditCustomer", "Account");
      }
    }
  }
}
