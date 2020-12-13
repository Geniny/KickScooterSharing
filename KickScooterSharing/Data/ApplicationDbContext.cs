using System;
using System.Collections.Generic;
using System.Text;
using KickScooterSharing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KickScooterSharing.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLocation> ProductLocations { get; set; }
        public DbSet<ParkingLocation> ParkingLocations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KickScooterSharing.Models.Tariff> Tariff { get; set; }

        public DbSet<KickScooterSharing.Models.Scooter> Scooter { get; set; }

        public DbSet<KickScooterSharing.Models.ScooterModel> ScooterModel { get; set; }

    }
}
