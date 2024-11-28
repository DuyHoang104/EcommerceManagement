using EcommerceManagement.Crosscutting.Emails.Dtos;

namespace EcommerceManagement.Crosscutting.Emails.Services
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(EmailDto emailDto);
    }
}
