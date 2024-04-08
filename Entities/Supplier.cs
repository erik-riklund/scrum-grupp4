using MongoDB.Entities;

namespace App.Entities
{
  public class Supplier : Entity
  {
    public string Name { get; set; }
    public string TelephoneNumber { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }

    [OwnerSide]
    public Many<Material, Supplier> Materials { get; set; }

    public Supplier()
    {
      this.InitOneToMany(() => Materials);
    }
  }
}
