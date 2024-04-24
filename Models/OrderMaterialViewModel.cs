using System.ComponentModel.DataAnnotations;
using App.Entities;

namespace App.Models
{
    public class OrderMaterialViewModel
    {
        public Material Material { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;
        public double Amount { get; set; }
    }
}
