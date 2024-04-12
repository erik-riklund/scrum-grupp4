using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class OrderMaterialViewModel
    {
        public string MaterialID { get; set; }
        public string MaterialName { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        [Required(ErrorMessage = "You need to enter the amount in Meters")]
        public double MaterialLength { get; set; }
        public string Name { get; set; } = null!;
        public string TelephoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
