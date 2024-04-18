using App.Entities;

namespace App.Models
{
    public class HandleOrderViewModel
    {
        public List<Hat> Hats {  get; set; }
        public string? OrderId { get; set; }
        public double orderSum { get; set; }

        public DateTime EstimatedDelivery { get; set; }
        public DateTime OrderDate { get; set; }
        public UserViewModel customer { get; set; }
        public string Status { get; set; }
        
    }
}
