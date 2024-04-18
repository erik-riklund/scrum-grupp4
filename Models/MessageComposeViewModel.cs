using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class MessageComposeViewModel
  {
    [Required]
    public string Topic { get; set; } = null!;

    [Required]
    public string Recipient { get; set; } = null!;

    [Required]
    public string Message { get; set; } = null!;
  }
}