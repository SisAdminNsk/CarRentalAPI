using CarRentalAPI.Application.Email;
using CarRentalAPI.Application.Interfaces.Email;
using ErrorOr;

namespace CarRentalAPI.Infrastructure.Email.Dependences
{
    public class CodeVerificationService : ICodeVerificationService
    {
        private static int VerificationCodeLifetimeInMinutes = 2;

        private readonly TimeSpan _codeExpirationTime = TimeSpan.FromMinutes(VerificationCodeLifetimeInMinutes);

        public ErrorOr<VerificationResult> Verify(VerificationCodeDetails serverCode, string userCode)
        {

            if (userCode == serverCode.Code)
            {
                if (serverCode.SendingTime.Add(_codeExpirationTime) >= DateTime.UtcNow)
                {
                    return VerificationResult.Verifed;
                }
                else
                {
                    return VerificationResult.Outdated;
                }
            }

            return VerificationResult.Wrong;
        }
        public TimeSpan GetCodeLifeTime()
        {
            return _codeExpirationTime;
        }
    }
}
