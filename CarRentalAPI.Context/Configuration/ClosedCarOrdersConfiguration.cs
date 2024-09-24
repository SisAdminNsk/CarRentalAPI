using CarRentalAPI.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Context.Configuration
{
    public class ClosedCarOrdersConfiguration : IEntityTypeConfiguration<ClosedCarOrder>
    {
        public void Configure(EntityTypeBuilder<ClosedCarOrder> builder)
        {
            builder.HasKey(cco => cco.Id);
            builder.HasOne(cco => cco.CarOrder);
        }
    }
}
