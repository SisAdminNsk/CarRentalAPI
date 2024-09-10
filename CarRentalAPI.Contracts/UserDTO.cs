namespace CarRentalAPI.Contracts
{
    public class UserDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public UserDTO(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
