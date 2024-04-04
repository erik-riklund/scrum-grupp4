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

		[OwnerSide]
		public Many<Hat, Order> Hats { get; set; }

		public Order() => this.InitOneToMany(() => Hats);
    }
}
