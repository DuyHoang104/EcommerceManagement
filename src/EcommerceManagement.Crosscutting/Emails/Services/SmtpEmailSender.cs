using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using EcommerceManagement.Crosscutting.Emails.Dtos;
using EcommerceManagement.Crosscutting.Emails.Services;
using EcommerceManagement.Crosscutting.Emails.Settings;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpConfig _smtpConfigs;

    public SmtpEmailSender(IOptions<SmtpConfig> smtpConfigs)
    {
        _smtpConfigs = smtpConfigs.Value;
    }

    public async Task SendEmailAsync(EmailDto emailDto)
    {
        using (var smtpClient = new SmtpClient(_smtpConfigs.Host, _smtpConfigs.Port))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpConfigs.UserName, _smtpConfigs.Password);
            smtpClient.EnableSsl = _smtpConfigs.EnableSsl;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpConfigs.UserName, "The EcommerceManagement Team"),
                Subject = emailDto.Subject,
                Body = emailDto.Message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDto.To);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

