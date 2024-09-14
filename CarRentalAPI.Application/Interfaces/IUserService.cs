using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<Success>> IsUserNotExistsWithData(UserRegistrateRequest data);
        Task<ErrorOr<Created>> RegistrateIfVerifiedAsync(
            UserRegistrateRequest userRegistrationRequest, 
            string code,
              CancellationToken cancellationToken);
        Task<ErrorOr<Success>> SendEmailVerificationCodeAsync(string email, CancellationToken cancellationToken);
        Task<ErrorOr<string>> LoginAsync(UserLoginRequest loginRequest);
    }
}
