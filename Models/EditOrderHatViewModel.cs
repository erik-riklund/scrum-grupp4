using App.Entities;

namespace App.Models
   
{
    public class EditOrderHatViewModel
    {
        public string OrderId { get; set; } = null!;
        public List<Material> Materials { get; set; } = null!;
        public string HatId { get; set; } = null!;
        public double Size { get; set; }
        public string HatDescription { get; set; } = null!;
        public string ProductCode { get; set; } = null!;
        public string imagePath { get; set; } = null!;

        public double Price { get; set; }

    }
}
