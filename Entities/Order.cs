using MongoDB.Entities;

namespace App.Entities
{
    public class Order : Entity
    {
        public string CustomerID { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime EstimatedDeliveryDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool IsApproved { get; set; }

        public string Status { get; set; } = null!;

        public double OrderSum { get; set; }

        public byte[] PDF { get; set; } = null!;

        [OwnerSide]
        public Many<Hat, Order> Hats { get; set; } = null!;

        [OwnerSide]
        public Many<Shipping, Order> Shippings { get; set; } = null!;

        public Order()
        {
            this.InitOneToMany(() => Hats);
            this.InitOneToMany(() => Shippings);
        }
    }
}