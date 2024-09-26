using CarRentalAPI.Core.Validation;
using CSharpFunctionalExtensions;

namespace CarRentalAPI.Core
{
    public class User : Entity<Guid>
    {
        [UsernameValidation]
        public string Name { get; set; }
        [EmailValidation]
        public string Email { get; set; }
        [PasswordValidation]
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
