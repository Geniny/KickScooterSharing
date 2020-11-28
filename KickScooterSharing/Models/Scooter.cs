using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickScooterSharing.Models
{
    public class Scooter
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string  Number { get; set; }
        public int? ScooterModelId { get; set; }
        public ScooterModel ScooterModel { get; set; }

        public List<Product> Products { get; set; }

        public Scooter()
        {
            this.Products = new List<Product>();
        }
    }
}
