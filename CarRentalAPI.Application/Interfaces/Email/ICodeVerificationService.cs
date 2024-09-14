using CarRentalAPI.Application.Email;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces.Email
{
    public interface ICodeVerificationService
    {
        ErrorOr<VerificationResult> Verify(string email, string userCode);

        TimeSpan GetCodeLifeTime();
    }
}
