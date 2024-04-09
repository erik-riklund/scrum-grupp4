using MongoDB.Entities;

namespace App.Entities
{
  public class Order : Entity
  {
    public DateTime EstimatedDeliveryDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public bool IsApproved { get; set; }
    public string Status { get; set; }
    public double OrderSum { get; set; }

    public Address Address { get; set; }

    public string CustomerId { get; set; }

    [OwnerSide]
    public Many<Shipping, Order> Shippings { get; set; }

    [OwnerSide]
    public Many<Hat, Order> Hats { get; set; }

    public Order()
    {
      this.InitOneToMany(() => Hats);
      this.InitOneToMany(() => Shippings);
    }
  }
}
