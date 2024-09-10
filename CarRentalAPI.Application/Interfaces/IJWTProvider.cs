using CarRentalAPI.Core;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IJWTProvider
    {
        string GenerateToken(User user);
    }
}
