using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class Role : Entity<Guid>
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }

        public Role()
        {
            Id = Guid.NewGuid();
        }
        public Role(Guid id, string name, List<User> users)
        {
            Id = id;
            Name = name;
            Users = users;
        }
    }
}
