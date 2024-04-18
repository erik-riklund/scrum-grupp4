using MongoDB.Entities;

namespace App.Entities
{
  public class Topic : Entity
  {
    public User Sender { get; set; } = null!;

    public User Recipient { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime Updated { get; set; } = DateTime.Now;

    public Many<Message, Topic> Messages { get; set; } = null!;

    public Topic() => this.InitOneToMany(() => Messages);
  }
}