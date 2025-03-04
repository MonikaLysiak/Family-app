using System.Net;
using System.Net.Mail;
using API.Helpers;
using API.Interfaces;
using Microsoft.Extensions.Options;

namespace API.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<SmtpSettings> _smtpSetings;

    public EmailService(IOptions<SmtpSettings> smtpSetings)
    {
        _smtpSetings = smtpSetings;
    }

    public async Task SendFromFamilyAppAsync(string to, string subject, string body)
    {
        var message = new MailMessage(_smtpSetings.Value.Sender,
            to,
            subject,
            body);

        using (var emailClient = new SmtpClient(_smtpSetings.Value.Host, _smtpSetings.Value.Port))
        {
            emailClient.Credentials = new  NetworkCredential(
                _smtpSetings.Value.Login,
                _smtpSetings.Value.Password
            );
            await emailClient.SendMailAsync(message);
        }
    }
}