using System.ComponentModel.DataAnnotations;
using App.Entities;

namespace App.Models
{
    public class OrderMaterialViewModel
    {
        public Material Material { get; set; }
        public Supplier Supplier { get; set; }
    }
}
