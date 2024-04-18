using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace App.Controllers
{
  public partial class MessageController
  {
    [HttpGet]
    public async Task<IActionResult> Topic(string id)
    {
      if (!string.IsNullOrWhiteSpace(id))
      {
        if (await Query.FetchOneById<Topic>(id) is Topic topic)
        {
          if (await session.GetUserAsync() is User user)
          {
            if (user.ID.Equals(topic.Recipient.ID) || user.ID.Equals(topic.Sender.ID))
            {
              var model = new MessageTopicViewModel
              {
                Topic = topic.ID,
                Title = topic.Title,
                Messages = topic.Messages.ToList()
              };

              foreach (var message in topic.Messages)
              {
                if (message.Sender.ID != user.ID)
                {
                  message.IsRead = true;
                  await message.SaveAsync();
                }
              }

              return View(model);
            }
          }
        }
      }

      return RedirectToAction("Index", "Message");
    }
  }
}