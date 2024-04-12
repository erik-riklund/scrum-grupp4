using MongoDB.Entities;

namespace App.Entities
{
  public class Hat : Entity
  {
    public Model Model { get; set; } = null!;

    public double Price { get; set; }

    public double Size { get; set; }

    public string Description { get; set; } = null!;
  }
}
