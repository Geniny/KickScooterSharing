using System.Collections.Generic;

namespace KickScooterSharing.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Status()
        {
            Users = new List<User>();
        }
    }
}
