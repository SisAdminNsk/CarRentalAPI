using CarRentalAPI.Core.Validation;

namespace CarRentalAPI.Contracts
{
    public class UserRegistrateRequest
    {
        [EmailValidation]
        public string Email { get; set; }  

        [UsernameValidation]
        public string Username { get; set; }
        [PasswordValidation]
        public string Password { get; set; }
        public UserRegistrateRequest(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }       
    }
}
