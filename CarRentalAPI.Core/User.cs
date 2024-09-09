using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class User : Entity<Guid>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }

        public User(
            Guid id,
            string login,
            string password,
            List<Role> roles)
        {
            Id = id;
            Login = login;
            Password = password;
            Roles = roles;
        }
    }
}
