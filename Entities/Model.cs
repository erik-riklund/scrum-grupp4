using MongoDB.Entities;

namespace App.Entities
{
  public class Model : Entity
  {
    public string ModelName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int Amount { get; set; }

    [OwnerSide]
    public Many<Material, Model> Materials { get; set; } = null!;

    public Model()
    {
      this.InitManyToMany(() => Materials, material => material.Models);
    }
  }
}
