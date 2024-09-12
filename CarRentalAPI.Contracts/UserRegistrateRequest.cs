using CarRentalAPI.Core.Validation;

namespace CarRentalAPI.Contracts
{
    public class UserRegistrateRequest
    {

        [UserLoginValidation]
        public string Login { get; set; }
        [UserPasswordValidation]
        public string Password { get; set; }
        public UserRegistrateRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
