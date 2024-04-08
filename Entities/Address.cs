using MongoDB.Entities;

namespace App.Entities
{
  public class Address : Entity
  {
    public string StreetAddress { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
  }
}
