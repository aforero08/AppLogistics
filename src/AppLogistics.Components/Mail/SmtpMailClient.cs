using Microsoft.Extensions.Configuration;
using SendGrid;
using System.Threading.Tasks;

namespace AppLogistics.Components.Mail;

public class SmtpMailClient : IMailClient
{
    private readonly IConfiguration _config;
    private readonly IMessagebuilder _messagebuilder;

    public SmtpMailClient(IConfiguration config, IMessagebuilder messagebuilder)
    {
        _config = config;
        _messagebuilder = messagebuilder;
    }

    public async Task SendFromAdmin(string recipientEmail, string recipientName, string subject, string body)
    {
        var apiKey = _config["Mail:SendGridApiKey"];
        var client = new SendGridClient(apiKey);

        var message = _messagebuilder.BuildMessageFromAdmin(recipientEmail, recipientName, subject, body, body);

        var response = await client.SendEmailAsync(message);
    }
}
