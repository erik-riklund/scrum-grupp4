using MongoDB.Entities;

namespace App.Entities
{
  public class Shipping : Entity
  {
    public string PackageWeight { get; set; }

    public string PackageQuantity { get; set; }

    public string PackageSize { get; set; }

    public string PackageContent { get; set; }

    public string ShippingCompany { get; set; }

    public string Payment { get; set; }
  }
}
