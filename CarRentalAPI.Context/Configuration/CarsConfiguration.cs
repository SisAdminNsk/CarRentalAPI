using CarRentalAPI.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Context.Configuration
{
    public class CarsConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        { 
            builder.HasKey(p => p.Id);

            builder.HasOne(c => c.OpenCarOrder).WithOne(co => co.Car);
        }
    }
}
