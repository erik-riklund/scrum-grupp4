using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class SpecialOrderViewModel
  {
    [Required(ErrorMessage = "Please enter a description for your hat")]
    public string Description { get; set; } = null!;
    
    [Required(ErrorMessage = "You need to enter your headsize")]
    [Range(40, 80, ErrorMessage = "Your head headsize must be between 40 and 80 cm.")]
    public double Size { get; set; }

    public IFormFile? Picture { get; set; }
  }
}
