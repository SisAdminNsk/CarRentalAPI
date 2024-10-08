﻿using MimeKit;

namespace CarRentalAPI.Infrastructure.Email.MessageTemplates
{
    public class VerificationMessage : MimeMessage
    {
        public VerificationMessage(string mailAgentName, string destMail, string code)
        {
            From.Add(new MailboxAddress("CarRentalService", mailAgentName));
            To.Add(new MailboxAddress("Получатель", destMail));

            Subject = "Код подтверждения";
            Body = new BodyBuilder() { HtmlBody = $"<div Код подтверждения: style=\"color: green;\">{code}</div>" }.ToMessageBody();
            
        }
    }
}
