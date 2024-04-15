using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class EditHatViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Please enter how big your head is in cm")]
        [Range(40, 80, ErrorMessage = "Your head headsize must be between 40 and 80 cm.")]
        public double Size { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
