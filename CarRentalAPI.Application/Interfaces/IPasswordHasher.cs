namespace CarRentalAPI.Application.Interfaces
{
    public interface IPasswordHasher
    {
        string Generate(string password);

        bool VerifyPassword(string password, string HashedPassword);
    }
}
