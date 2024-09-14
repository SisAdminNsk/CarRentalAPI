using CarRentalAPI.Application.Email;
using ErrorOr;

namespace CarRentalAPI.Application.Interfaces.Email
{
    public interface IConcurrentVerificationCodesStorage
    {
        public ErrorOr<VerificationCodeDetails> TryGetCode(string key);
        public ErrorOr<Success> TryAddCode(string email, VerificationCodeDetails code);
        public ErrorOr<Success> TryDeleteCode(string email);
        public ErrorOr<int> TryClearFromOutdatedCodes(TimeSpan codeLifetime);

        public int GetCodesCount();
    }
}
