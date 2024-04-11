using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class SupplierViewModel
    {
        public string SupplierName { get; set; } 
        
        public string Email { get; set; }

        public string TelephoneNumber { get; set;}

        public string StreetAddress { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;





    }
}
