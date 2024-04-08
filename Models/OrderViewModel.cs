using App.Entities;
using System.Transactions;

namespace App.Models
{
    public class OrderViewModel
    {
        public double Size { get; set; }
        public string ModelID { get; set; }

        public string Description { get; set; }

    }
}
