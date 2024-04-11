using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class SupplierViewModel
    {
        public string SupplierName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string TelephoneNumber { get; set;} = null!;

        public string StreetAddress { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;





    }
}
