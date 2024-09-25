using CarRentalAPI.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalAPI.Context.Configuration
{
    public sealed class CarsharingUsersConfiguration : IEntityTypeConfiguration<CarsharingUser>
    {
        public void Configure(EntityTypeBuilder<CarsharingUser> builder)
        {
            builder.HasMany(cu => cu.Orders).WithOne(or => or.CarsharingUser);
        }
    }
}
