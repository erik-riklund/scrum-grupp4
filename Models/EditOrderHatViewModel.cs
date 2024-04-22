using App.Entities;

namespace App.Models
   
{
    public class EditOrderHatViewModel
    {
        public string OrderId { get; set; }
        public List<Material> Materials { get; set; }
        public string HatId { get; set; }
        public double Size { get; set; }
        public string HatDescription { get; set; }
        public string ProductCode { get; set; }
        public string imagePath { get; set; }

        public double Price { get; set; }

    }
}
