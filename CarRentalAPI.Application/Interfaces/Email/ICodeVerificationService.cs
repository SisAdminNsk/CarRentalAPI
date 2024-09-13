using CarRentalAPI.Application.Email;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces.Email
{
    public interface ICodeVerificationService
    {
        ErrorOr<VerificationResult> Verify(VerificationCodeDetails serverCode, string userCode);

        TimeSpan GetCodeLifeTime();
    }
}
