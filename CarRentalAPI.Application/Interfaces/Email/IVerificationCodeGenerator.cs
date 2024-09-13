namespace CarRentalAPI.Application.Interfaces.Email
{
    public interface IVerificationCodeGenerator
    {
        string GenerateCode();
    }
}
