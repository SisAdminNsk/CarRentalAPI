using CarRentalAPI.Application.Email;
using CarRentalAPI.Contracts;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<ErrorOr<Success>> IsUserNotExistsWithData(UserRegistrateRequest data);
        Task<ErrorOr<Created>> RegistrateIfVerifiedAsync(
            VerificationCodeDetails serverCode,
            UserRegistrateRequest userRegistrationRequest, 
            string userCode,
              CancellationToken cancellationToken);
        Task<ErrorOr<VerificationCodeDetails>> SendEmailVerificationCodeAsync(string email, CancellationToken cancellationToken);
        Task<ErrorOr<string>> LoginAsync(UserLoginRequest loginRequest);
    }
}
