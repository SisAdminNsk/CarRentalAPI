using CarRentalAPI.Application.Email;
using CarRentalAPI.Application.Interfaces.Email;
using ErrorOr;

namespace CarRentalAPI.Infrastructure.Email.Dependences
{
    public class CodeVerificationService : ICodeVerificationService
    {
        private static int VerificationCodeLifetimeInMinutes = 5;

        private IConcurrentVerificationCodesStorage _codesStorage;

        private readonly TimeSpan _codeExpirationTime = TimeSpan.FromMinutes(VerificationCodeLifetimeInMinutes);

        public CodeVerificationService(IConcurrentVerificationCodesStorage codesStorage)
        {
            _codesStorage = codesStorage;
        }

        public ErrorOr<VerificationResult> Verify(string email, string userCode)
        {
            var result = _codesStorage.TryGetCode(key: email);

            if (result.IsError)
            {
                return result.Errors;
            }

            var serverCode = result.Value;

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
