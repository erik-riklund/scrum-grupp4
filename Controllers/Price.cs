using App.Entities;

namespace App.Controllers
{
    public class Price
    {

        public double GetHatPrice(Dictionary<Material, double> materialsAmount)
        {
            double price = 0;
            foreach (var material in materialsAmount.Keys) 
            {
                if (material.Unit.Equals("Meter"))
                {
                    double amountMeter = materialsAmount[material]/100;
                    price += amountMeter*material.Price;
                }
                else
                {
                    price += material.Price*materialsAmount[material];
                }
            }
            return price;
        }
    }
}
