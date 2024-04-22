namespace App.Models
{
    public class OrderHistoryViewModel
    {
        public DateTime DateTo {  get; set; }

        public DateTime DateFrom { get; set; }

        public bool PaymentStatus { get; set; }

        public bool ChooseDate { get; set; }

    }
}
