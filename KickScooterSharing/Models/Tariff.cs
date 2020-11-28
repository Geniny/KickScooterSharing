using System;
using System.Collections.Generic;

namespace KickScooterSharing.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CostPerMinute { get; set; }
        public double StartPrice { get; set; }
        public double BookingCostPerMinute { get; set; }
        public List<Product> Products { get; set; }

        public Tariff()
        {
            this.Products = new List<Product>();
        }
    }
}
