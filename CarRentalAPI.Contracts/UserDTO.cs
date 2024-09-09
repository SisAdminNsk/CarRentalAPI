namespace CarRentalAPI.Contracts
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserDTO( string login, string password, Guid id)
        {
            Login = login;
            Password = password;
            Id = id;
        }
    }
}
