using MongoDB.Entities;

namespace App.Entities
{
	public class Model : Entity
	{
		public string ModelName { get; set; }
		public string Description { get; set; }
		public string Picture { get; set; }
		public string ProductCode { get; set; }

		[OwnerSide]
		public Many<Material, Model> Materials { get; set; }

		[OwnerSide]
		public Many<Hat, Model> Hats { get; set; }

		public Model()
		{
			this.InitManyToMany(() => Materials, material => material.Models);
			this.InitOneToMany(() => Hats);
		}
    }
}
