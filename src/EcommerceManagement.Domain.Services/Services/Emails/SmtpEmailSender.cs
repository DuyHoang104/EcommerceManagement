using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using EcommerceManagement.Crosscutting.Dtos.Emails;
using EcommerceManagement.Domain.Repositories;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public SmtpEmailSender(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(EmailServiceDto emailDto)
    {
        using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
            smtpClient.EnableSsl = _smtpSettings.EnableSsl;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.UserName, "The EcommerceManagement Team"),
                Subject = emailDto.Subject,
                Body = emailDto.Message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDto.Email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
