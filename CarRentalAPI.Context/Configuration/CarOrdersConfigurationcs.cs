using CarRentalAPI.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Context.Configuration
{
    public class CarOrdersConfigurationcs : IEntityTypeConfiguration<CarOrder>
    {
        public void Configure(EntityTypeBuilder<CarOrder> builder)
        {
            builder.HasKey(co => co.Id);
            builder.HasOne(co => co.Car);
            builder.HasOne(co => co.Customer).WithMany(cus => cus.Orders);
        }
    }
}
