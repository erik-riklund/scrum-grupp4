using App.Entities;

namespace App.Models
{
  public class MessageTopicViewModel
  {
    public string Topic { get; set;} = null!;

    public string Title { get; set;} = null!;

    public List<Message> Messages { get; set; } = [];
  }
}