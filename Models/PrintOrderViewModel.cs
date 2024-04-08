namespace App.Models
{
    public class PrintOrderViewModel
    {
        public string OrderNumber { get; set; }
        public int PackageWeight { get; set; }

        public int PackageQuantity { get; set; }

        public int PackageSize { get; set; }

        public string PackageContent { get; set; }

        public string ShippingCompany { get; set; }

        public string Payment { get; set; }
    }
}
