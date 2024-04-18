using App.Entities;

namespace App.Models
{
  public class MessageListViewModel
  {
    public IOrderedEnumerable<Topic> Topics { get; set; } = null!;
  }
}