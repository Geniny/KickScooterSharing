using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KickScooterSharing.Models
{
    public class User : IdentityUser
    {
        public int? StatusId { get; set; }
        public Status Status { get; set; }
        public int? SecondNameId { get; set; }
        public SecondName SecondName { get; set; }
        public int? FirstNameId { get; set; }
        public FirstName FirstName { get; set; }
        public DateTime RegisterDate { get; set; }
        public double Balance { get; set; }
        public List<Sales> Sales { get; set; }

        public User()
        {
            this.Sales = new List<Sales>();
        }
    }
}
