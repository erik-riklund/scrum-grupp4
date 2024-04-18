namespace App.Models
{
    public class ModelViewModel
    {
        public string ModelName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public string ProductCode { get; set; } = null!;
        public int Amount { get; set; }
        public Dictionary<string, double> MaterialUsed { get; set; } = new Dictionary<string, double>();
    }
}
