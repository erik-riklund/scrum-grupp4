using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class RegisterAdminViewModel
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

        public string RoleName { get; set; }
    }
}
