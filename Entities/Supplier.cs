using MongoDB.Entities;

namespace App.Entities
{
  public class Supplier : Entity
  {
    public string Name { get; set; } = null!;

    public string TelephoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Address Address { get; set; } = null!;

    [OwnerSide]
    public Many<Material, Supplier> Materials { get; set; } = null!;

    public Supplier() => this.InitOneToMany(() => Materials);
  }
}
