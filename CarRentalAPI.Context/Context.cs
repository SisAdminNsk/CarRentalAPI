using CarRentalAPI.Context.Configuration;
using CarRentalAPI.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRentalAPI.Context
{
    public class Context : DbContext, IContext
    {
        public DbSet<ClosedCarOrder> ClosedCarOrders { get; set; }
        public DbSet<OpenCarOrder> OpenCarOrders { get; set; }
        public DbSet<CarOrder> CarOrders { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Car> Cars { get; set; } = null!;

        public DbSet<Role> UsersRoles { get; set; } = null!;

        public DbSet<CarsharingUser> CarsharingUsers { get; set; } = null!;

        protected Context(DbContextOptions options) : base(options)
        {

        }

        protected Context() { AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new CarsharingUsersConfiguration());
            modelBuilder.ApplyConfiguration(new CarsConfiguration());
            modelBuilder.ApplyConfiguration(new CarOrdersConfigurationcs());
            modelBuilder.ApplyConfiguration(new OpenCarOrdersConfiguration());
            modelBuilder.ApplyConfiguration(new ClosedCarOrdersConfiguration());
        }

        public ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
