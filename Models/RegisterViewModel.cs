using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phonenumber { get; set; }

        [Required, Compare("Password")]
        public string RepeatPassword { get; set; }

        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Country { get; set; }
    }
}
