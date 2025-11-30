using Microsoft.AspNetCore.Identity.UI.Services;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // the logic to send an email must be here
            return Task.CompletedTask;
        }
    }
}
