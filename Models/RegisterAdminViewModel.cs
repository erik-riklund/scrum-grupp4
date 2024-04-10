using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class RegisterAdminViewModel
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
  }
}
