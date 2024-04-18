using App.Entities;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
  public partial class MessageController
  {
    public async Task<IActionResult> Index()
    {
      var model = new MessageListViewModel();

      if (await session.GetUserAsync() is User user)
      {
        var topics = await Query.FetchMany<Topic>(
          topic => topic.Recipient.ID == user.ID || topic.Sender.ID == user.ID
          );

        model.Topics = ((IEnumerable<Topic>)topics).OrderByDescending((Topic topic) => topic.Updated);
      }

      return View(model);
    }
  }
}