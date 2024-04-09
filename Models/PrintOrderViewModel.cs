﻿using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class PrintOrderViewModel
  {
    public string OrderNumber { get; set; }

    [Required]
    public string PackageWeight { get; set; }

    [Required]
    public string PackageQuantity { get; set; }

    [Required]
    public string PackageSize { get; set; }

    [Required]
    public string PackageContent { get; set; }

    [Required]
    public string ShippingCompany { get; set; }

    [Required]
    public string Payment { get; set; }
  }
}
