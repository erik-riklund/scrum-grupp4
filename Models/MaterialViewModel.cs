namespace App.Models
{
  public class MaterialViewModel
  {
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string SupplierID { get; set; } = null!;

    public double Price { get; set; }

    public string Unit { get; set; } = null!;
    
    public double CurrentAmount { get; set; }
  }
}
