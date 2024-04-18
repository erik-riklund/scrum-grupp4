using App.Models;
using App.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Entities;
using MongoDB.Driver.Linq;

namespace App.Controllers
{
  public partial class MessageController
  {
    [HttpGet]
    public async Task<IActionResult> Compose(string? reciever = null)
    {
      var model = new MessageComposeViewModel();

      var self = (await session.GetUserAsync())?.ID;
      var users = ((IEnumerable<User>)await Query.FetchMany<User>(user => !user.ID.Equals(self))).ToList();

      ViewBag.Users = users.Select(user => new SelectListItem { Value = user.ID, Text = $"{user.FirstName} {user.LastName}" });

      if (reciever != null)
      {
        model.Recipient = users.Where(user => user.ID.Equals(reciever)).FirstOrDefault()?.ID ?? string.Empty;
      }

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Compose(MessageComposeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        if (await Query.FetchOneById<User>(model.Recipient) is User recipient)
        {
          try
          {
            if (await session.GetUserAsync() is User sender)
            {
              var message = new Message
              {
                Sender = sender,
                Content = model.Message
              };

              await message.SaveAsync();

              var topic = new Topic
              {
                Sender = sender,
                Recipient = recipient,
                Title = model.Topic
              };

              await topic.SaveAsync();
              await topic.Messages.AddAsync(message);

              return RedirectToAction("Topic", "Message", new { id = topic.ID });
            }
            else ModelState.AddModelError(string.Empty, "Unable to load the sender. Please try again.");
          }

          catch (Exception)
          {
            ModelState.AddModelError(string.Empty, "Failed to complete the request, try again.");
          }
        }
        else ModelState.AddModelError(string.Empty, "The specified recipient was not found.");
      }

      return View(model);
    }
  }
}