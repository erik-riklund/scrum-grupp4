using System.ComponentModel.DataAnnotations;

namespace App.Models
{
  public class MessageReplyViewModel
  {
    public string Topic { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Recipient { get; set; } = null!;

    [Required]
    public string Message { get; set; } = null!;
  }
}