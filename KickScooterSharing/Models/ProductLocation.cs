using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickScooterSharing.Models
{
    public class ProductLocation
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
