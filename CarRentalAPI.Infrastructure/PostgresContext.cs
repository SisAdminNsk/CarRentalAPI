using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarRentalAPI.Infrastructure
{
    public class PostgresContext : Context.Context
    {
        protected readonly IConfiguration? _configuration;

        public PostgresContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public PostgresContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = _configuration.GetRequiredSection("Connection");
            optionsBuilder.UseNpgsql(options.Value)
                .UseLoggerFactory(CreateLoggerFactory())
                .EnableSensitiveDataLogging();
        }
    }
}
