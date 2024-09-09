using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<ErrorOr<Created>> RegistrateNewUserAsync(UserDTO user);
    }
}
