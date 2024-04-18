using MongoDB.Entities;

namespace App.Entities
{
  public class Message : Entity
  {
    public User Sender { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Posted { get; set; } = DateTime.Now;

    public bool IsRead { get; set; } = false;
  }
}