using CarRentalAPI.Core;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Context
{
    public interface IContext
    {
        public DbSet<User> Users { get; }

        public DbSet<Car> Cars { get; }

        public DbSet<Role> UsersRoles { get; }

        public DbSet<CarsharingUser> CarsharingUsers { get; }
    }
}
