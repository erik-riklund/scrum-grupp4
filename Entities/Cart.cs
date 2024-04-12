using MongoDB.Entities;

namespace App.Entities

{
    public class Cart: Entity
    {
        public string UserID { get; set; }

        [OwnerSide]
        public Many<Hat, Cart> Hats { get; set; } = null!;

        public double TotalSum { get; set; }

        public Cart()
        {
            this.InitOneToMany(() => Hats);
        }
        public void UpdateTotalSum()
        {
            double sum = 0;
            if (Hats.Count() > 0 && Hats != null)
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
