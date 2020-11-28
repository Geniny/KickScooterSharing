using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickScooterSharing.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TariffId { get; set; }
        public Tariff Tariff { get; set; }
        public int? ScooterId { get; set; }
        public Scooter Scooter { get; set; }
        public int? StatusId { get; set; }
        public Status Status { get; set; }
        public List<Sales> Sales { get; set; }
        public List<ProductLocation> ProductLocations { get; set; }
        public Product()
        {
            this.Sales = new List<Sales>();
            this.ProductLocations = new List<ProductLocation>();
        }

    }
}
