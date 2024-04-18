using App.Models;
using App.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MessageController
  {
    [HttpGet]
    public async Task<IActionResult> Reply(string? id = null)
    {
      if (!string.IsNullOrEmpty(id))
      {
        if (await Query.FetchOneById<Topic>(id) is Topic topic)
        {
          if (await session.GetUserAsync() is User user)
          {
            var recipient = topic.Sender.ID == user.ID ?
              $"{topic.Recipient.FirstName} {topic.Recipient.LastName}" :
              $"{topic.Sender.FirstName} {topic.Sender.LastName}";

            return View(new MessageReplyViewModel
            {
              Topic = topic.ID,
              Title = topic.Title,
              Recipient = recipient
            });
          }
        }
      }

      return RedirectToAction("Index", "Message");
    }

    [HttpPost]
    public async Task<IActionResult> Reply(MessageReplyViewModel model)
    {
      if (ModelState.IsValid)
      {
        if (await session.GetUserAsync() is User user)
        {
          if (await Query.FetchOneById<Topic>(model.Topic) is Topic topic)
          {
            topic.Updated = DateTime.Now;
            await topic.SaveAsync();

            var message = new Message
            {
              Sender = user,
              Content = model.Message
            };

            await message.SaveAsync();
            await topic.Messages.AddAsync(message);

            return Redirect(Url.Action(
              "Topic", "Message", new { id = model.Topic }) + $"#{message.ID}"
            );
          }
        }

        ModelState.AddModelError(
          string.Empty, "Unable to save the reply, please try again."
        );
      }

      return View(model);
    }
  }
}