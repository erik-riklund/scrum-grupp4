using MongoDB.Entities;

namespace App.Entities
{
  public class Material : Entity
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public string SupplierID { get; set; }

    public double Price { get; set; }

    public string Unit { get; set; }
    
    public double CurrentAmount { get; set; }

    [InverseSide]
    public Many<Model, Material> Models { get; set; }

    public Material() => this.InitManyToMany(() => Models, model => model.Materials);
  }
}
