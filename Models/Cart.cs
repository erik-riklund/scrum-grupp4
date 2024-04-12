using App.Entities;

namespace App.Models

{
    public class Cart
    {
        public User Customer { get; set; }
        public List<Hat> Hats { get; set; }

        public double TotalSum { get; set; }

        public void AddHat(Hat hat)
        {
            Hats.Add(hat);
            UpdateTotalSum();   
        }

        public void RemoveHat(Hat hat)
        {
            Hats.Remove(hat);
            UpdateTotalSum();
        }

        public void UpdateTotalSum()
        {
            double sum = 0;
            if (Hats.Count > 0) 
            {
                foreach (Hat hat in Hats)
                {
                    sum += hat.Price;
                }
            }
            else { sum = 0; }
            TotalSum = sum;
        }

    }
}
