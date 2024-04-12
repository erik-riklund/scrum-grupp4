
using App.Entities;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace App.Models

{
    public class HatViewModel
    {
        [Required]
        public Model HatModel { get; set; }
        [Required(ErrorMessage = "Please enter how big your head is in cm")]
        public double Size { get; set; } 
        public string? Description { get; set; }
    }
}
