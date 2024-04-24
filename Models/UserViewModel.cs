namespace App.Models
{
    public class UserViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string id { get; set; } = null!;
        public string Email { get; set; }   = null!;
        public string StreetAdress { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
