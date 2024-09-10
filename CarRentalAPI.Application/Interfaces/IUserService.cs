using CarRentalAPI.Contracts;
using CarRentalAPI.Core;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<Created>> RegistrateNewUserAsync(UserDTO user);

        Task<ErrorOr<List<Role>>> GetRolesByLoginAsync(string login);

        Task<ErrorOr<string>> LoginAsync(UserLoginRequest loginRequest);
    }
}
