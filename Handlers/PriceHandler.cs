﻿using App.Entities;

namespace App.Handlers
{
    public class PriceHandler
    {
        public double GetHatPrice(Dictionary<Material, double> materialsAmount)
        {
            double price = 0;
            foreach (var material in materialsAmount.Keys)
            {
                if (material.Unit.Equals("Meter"))
                {
                    double amountMeter = materialsAmount[material] / 100;
                    price += amountMeter * material.Price;
                }
                else
                {
                    price += material.Price * materialsAmount[material];
                }
            }

            double fastAvgift = 2500;
            price += fastAvgift;
            double accuratePrice = Convert.ToDouble(Math.Round(price));

            return accuratePrice;
        }
    }
}
