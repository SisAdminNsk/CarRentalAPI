using CarRentalAPI.Application.Interfaces.Email;

namespace CarRentalAPI.Infrastructure.Email.Dependences
{
    public class Verification6DigitCodeGenerator : IVerificationCodeGenerator
    {
        public string GenerateCode()
        {
            var random = new Random();

            int code = random.Next(100000, 999999);

            return code.ToString();
        }
    }
}
