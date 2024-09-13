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
        private ICodeVerificationService _codeVerificationService;

        private EmailSender _emailSender;
        public EmailConfirmationService(
            IConfiguration configuration,
            IVerificationCodeGenerator verificationCodeGenerator,
            ICodeVerificationService codeVerificationService)
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
        }

        public ErrorOr<VerificationResult> VerifyCode(VerificationCodeDetails serverCode, string userCode)
        {
            return _codeVerificationService.Verify(serverCode, userCode);
        }

        public async Task<ErrorOr<VerificationCodeDetails>> SendRegistrationCodeMessageAsync(string destEmail,
            CancellationToken cancellationToken)
        {
            var verificationCode = new VerificationCodeDetails(_verificationCodeGenerator.GenerateCode(), DateTime.UtcNow);

            var registrationCodeMessange = new VerificationMessage(_emailSender.GetEmailAgentName(), destEmail, verificationCode.Code);

            var result = await _emailSender.SendEmailMessageAsync(registrationCodeMessange, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            return verificationCode;
        }
    }
}
