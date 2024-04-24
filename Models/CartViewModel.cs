using App.Entities;
namespace App.Models
{
    public class CartViewModel 
    {
        public List<Hat> hats {  get; set; } = null!;
        public Cart cart { get; set; } = null!;
    }
}
