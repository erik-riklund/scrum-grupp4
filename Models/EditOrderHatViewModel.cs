namespace App.Models
{
    public class EditOrderHatViewModel
    {
        public string OrderId { get; set; }
        public List<AddMaterialViewModel> Materials { get; set; }
        public string HatId { get; set; }
        public double Size { get; set; }
        public string HatDescription { get; set; }
        public string ProductCode { get; set; }


    }
}
