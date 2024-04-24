namespace App.Models
{
    public class AddMaterialViewModel
    {
        public string materialId { get; set; } = null!;
        public string materialName { get; set; } = null!;
        public double amount { get; set; }
        public bool inHat { get; set; }
    }
}
