using App.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Models
{
    public class ApproveOrderViewModel
    {
        public List<Order> unapprovedOrders { get; set; } = new List<Order>();
        public List<Order> approvedOrders { get; set; } = new List<Order>();

      
    }
}

