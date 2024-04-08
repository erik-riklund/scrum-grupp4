using MongoDB.Entities;

namespace App.Entities
{
  public class Hat : Entity
  {
    public string ModelID { get; set; }
    public double Price { get; set; }
    public double Size { get; set; }

    public string Description { get; set; }
  }
}
