using CarRentalAPI.Application.Email;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces.Email
{
    public interface IEmailConfirmationService
    {
        Task<ErrorOr<VerificationCodeDetails>> SendRegistrationCodeMessageAsync(string destEmail, CancellationToken cancellationToken);

        ErrorOr<VerificationResult> VerifyCode(VerificationCodeDetails serverCode, string userCode);
    }
}
