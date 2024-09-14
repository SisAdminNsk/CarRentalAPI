using CarRentalAPI.Application.Email;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces.Email
{
    public interface IEmailConfirmationService
    {
        Task<ErrorOr<Success>> SendRegistrationCodeMessageAsync(string destEmail, CancellationToken cancellationToken);

        ErrorOr<VerificationResult> VerifyCode(string email, string code);
    }
}
