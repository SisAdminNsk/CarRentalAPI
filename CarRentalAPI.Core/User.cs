using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class User : Entity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }

        public User(
            Guid id,
            string email,
            string password,
            string name,
            List<Role> roles)
        {
            Id = id;
            Email = email;
            Password = password;
            Roles = roles;
            Name = name;
        }
    }
}
