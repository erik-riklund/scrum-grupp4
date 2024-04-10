using MongoDB.Entities;

namespace App.Entities
{
  public class Material : Entity
  {
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string SupplierID { get; set; } = null!;

    public double Price { get; set; }

    public string Unit { get; set; } = null!;
    
    public double CurrentAmount { get; set; }

    [InverseSide]
    public Many<Model, Material> Models { get; set; } = null!;

    public Material() => this.InitManyToMany(() => Models, model => model.Materials);
  }
}
