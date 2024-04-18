namespace App.Models
{
    public class EditCustomerViewModel
    {
        public string CustomerId { get; set; } = null!;
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public string StreetAddress { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

    }
}
