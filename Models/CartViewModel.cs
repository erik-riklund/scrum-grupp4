using App.Entities;
namespace App.Models
{
    public class CartViewModel 
    {
        public List<Hat> hats {  get; set; }
        public Cart cart { get; set; }
    }
}
