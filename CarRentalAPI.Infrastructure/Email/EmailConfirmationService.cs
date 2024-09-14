using CarRentalAPI.Application.Email;
using CarRentalAPI.Application.Interfaces.Email;
using CarRentalAPI.Infrastructure.Email.MessageTemplates;

using EmailClient;
using ErrorOr;
using Microsoft.Extensions.Configuration;

namespace CarRentalAPI.Infrastructure.Email
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private IVerificationCodeGenerator _verificationCodeGenerator;
        private IConcurrentVerificationCodesStorage _verificationCodesStorage;
        private ICodeVerificationService _codeVerificationService;

        private EmailSender _emailSender;
        public EmailConfirmationService(
            IConfiguration configuration,
            IVerificationCodeGenerator verificationCodeGenerator,
            ICodeVerificationService codeVerificationService,
            IConcurrentVerificationCodesStorage verificationCodesStorage)
        {
            var emailOptions = configuration.GetRequiredSection("EmailOptions");

            var hostDetails = new HostDetails
            (
                hostName: emailOptions.GetRequiredSection("Host").Value,
                hostPort: int.Parse(emailOptions.GetRequiredSection("Port").Value)
            );

            var emailAgentDetails = new EmailAgentCredentials
            (
                name: emailOptions.GetRequiredSection("AgentName").Value,
                password: emailOptions.GetRequiredSection("AgentPassword").Value
            );

            _emailSender = new EmailSender(hostDetails, emailAgentDetails);

            _verificationCodeGenerator = verificationCodeGenerator;
            _codeVerificationService = codeVerificationService;
            _verificationCodesStorage = verificationCodesStorage;
        }

        public ErrorOr<VerificationResult> VerifyCode(string email, string code)
        {
            return _codeVerificationService.Verify(email, code);
        }

        public async Task<ErrorOr<Success>> SendRegistrationCodeMessageAsync(string destEmail,
            CancellationToken cancellationToken)
        {
            var verificationCode = new VerificationCodeDetails(_verificationCodeGenerator.GenerateCode(), DateTime.UtcNow);

            var registrationCodeMessange = new VerificationMessage(_emailSender.GetEmailAgentName(), destEmail, 
                verificationCode.Code);

            var result = await _emailSender.SendEmailMessageAsync(registrationCodeMessange, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            try
            {
                _verificationCodesStorage.TryAddCode(email: destEmail, code: verificationCode);

            }
            catch (Exception ex)
            {
                return Error.Failure(description: "Failure on adding verification code to the server");
            }
            finally
            {
                ClearFromOutdatedCodes();
            }

            return Result.Success;
        }

        private void ClearFromOutdatedCodes()
        {
            int maxVerificationCodesCount = 100;

            if (_verificationCodesStorage.GetCodesCount() >= maxVerificationCodesCount)
            {
                _verificationCodesStorage.TryClearFromOutdatedCodes(_codeVerificationService.GetCodeLifeTime());
            }
        }
    }
}
