namespace CarRentalAPI.Contracts
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }

        public LoginResponse(string token, Guid userId)
        {
            Token = token;
            UserId = userId;
        }

        public LoginResponse()
        {

        }
    }
}
