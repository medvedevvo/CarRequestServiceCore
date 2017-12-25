using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarRequestService;

namespace CarRequestService.Models
{
    public class CarServicesDbContext : DbContext
    {
        private ConnectionSettings connectionSettings = ConnectionSettings.getInstance();

        public DbSet<CarService> CarServices { get; set; }

        public CarServicesDbContext() : base() { }
        public CarServicesDbContext(DbContextOptions<CarServicesDbContext> ops) : base(ops) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(connectionSettings.ConnectionString);
            base.OnConfiguring(builder);
        }
    }
}
