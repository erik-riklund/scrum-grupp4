using MongoDB.Entities;

namespace App.Entities
{
	public class Hat : Entity
	{
		public string ModelID { get; set; }
		public double Price { get; set; }
		public double Size { get; set; }

        public Hat(string modelID, double price, double size) : base()
        {
            ModelID = modelID;
			Price = price;
			Size = size;
        }
    }
}
