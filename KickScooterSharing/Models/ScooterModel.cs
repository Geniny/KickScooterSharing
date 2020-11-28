using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KickScooterSharing.Models
{
    public class ScooterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Scooter> Scooters { get; set; }
        public ScooterModel()
        {
            this.Scooters = new List<Scooter>();
        }
    }
}
