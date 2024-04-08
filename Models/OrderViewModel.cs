using App.Entities;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace App.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "You need to enter your headsize")]
        [Range(40, 80, ErrorMessage = "Your head headsize must be between 40 and 80 cm.")]
        public double Size { get; set; }
        public string ModelID { get; set; }

        public string Description { get; set; }

    }
}
