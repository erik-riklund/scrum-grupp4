using MongoDB.Entities;

namespace App.Entities
{
  public class Shipping : Entity
  {
    public string PackageWeight { get; set; } = null!;

    public string PackageQuantity { get; set; } = null!;

    public string PackageSize { get; set; } = null!;

    public string PackageContent { get; set; } = null!;

    public string ShippingCompany { get; set; } = null!;

    public string Payment { get; set; } = null!;
  }
}
