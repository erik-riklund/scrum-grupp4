using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class PrintOrderViewModel
  {
    public string OrderNumber { get; set; } = null!;

    [Required]
    public string PackageWeight { get; set; } = null!;

    [Required]
    public string PackageQuantity { get; set; } = null!;

    [Required]
    public string PackageSize { get; set; } = null!;

    [Required]
    public string PackageContent { get; set; } = null!;

    [Required]
    public string ShippingCompany { get; set; } = null!;

    [Required]
    public string Payment { get; set; } = null!;
  }
}
