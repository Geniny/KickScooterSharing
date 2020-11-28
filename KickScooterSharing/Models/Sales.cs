using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickScooterSharing.Models
{
    public class Sales
    {
        public int? Id { get; set; }
        public float Price { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
