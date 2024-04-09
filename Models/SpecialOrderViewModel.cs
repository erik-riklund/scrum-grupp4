namespace App.Models
{
  public class SpecialOrderViewModel
  {
    public string Description { get; set; }
    public double Size { get; set; }
    public List<string> Materials { get; set; }

    public IFormFile Picture { get; set; }
  }
}
