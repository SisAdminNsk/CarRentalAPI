using CarRentalAPI.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Context.Configuration
{
    public class OpenCarOrdersConfiguration : IEntityTypeConfiguration<OpenCarOrder>
    {
        public void Configure(EntityTypeBuilder<OpenCarOrder> builder)
        {
            builder.HasKey(cco => cco.Id);
            builder.HasOne(cco => cco.CarOrder);

            builder.HasOne(cco => cco.Car).WithOne(c => c.OpenCarOrder);
        }
    }
}
