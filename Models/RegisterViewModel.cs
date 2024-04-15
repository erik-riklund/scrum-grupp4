using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class RegisterViewModel
  {
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Phonenumber { get; set; } = null!;

    [Required, Compare("Password")]
    public string RepeatPassword { get; set; } = null!;

    [Required]
    public string StreetAddress { get; set; } = null!;

    [Required]
    public string ZipCode { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;
  }
}
