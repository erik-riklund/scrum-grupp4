
using App.Entities;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace App.Models

{
    public class HatViewModel
    {
        [Required]
        public string modelID { get; set; } = null!;
        [Required(ErrorMessage = "Please enter how big your head is in cm")]
        [Range(40, 80, ErrorMessage = "Your head headsize must be between 40 and 80 cm.")]
        public double Size { get; set; } 
        public string? Description { get; set; }
    }
}
