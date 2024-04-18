using App.Entities;

namespace App.Models
{
    public class OrderCustomerViewModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string StreetAddress { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string ModelID { get; set; } = null!;

        public double Price { get; set; }

        public double Size { get; set; }
        
        public string Description { get; set; } = null!;
    }
}