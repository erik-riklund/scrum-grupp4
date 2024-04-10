using MongoDB.Entities;

namespace App.Entities
{
  public class Address : Entity
  {
    public string StreetAddress { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;
  }
}
