using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;

namespace AppLogistics.Components.Mail;

public class MessageBuilder : IMessagebuilder
{
    private readonly IConfiguration _config;

    public MessageBuilder(IConfiguration config)
    {
        _config = config;
    }

    public SendGridMessage BuildMessageFromAdmin(string recipientEmail, string recipientName, string subject,
        string htmlContent, string plainTextContent)
    {
        var adminEmail = _config["Mail:AdminSenderEmail"];
        var adminName = _config["Mail:AdminSenderName"];

        return BuildMessage(adminEmail, adminName, recipientEmail, recipientName, subject, htmlContent, plainTextContent);
    }

    public SendGridMessage BuildMessage(string senderEmail, string senderName, string recipientEmail,
        string recipientName, string subject, string htmlContent, string plainTextContent)
    {
        var fromAddress = new EmailAddress(senderEmail, string.IsNullOrWhiteSpace(senderName) ? null : senderName);
        var toAddress = new EmailAddress(recipientEmail, string.IsNullOrEmpty(recipientName) ? null : recipientName);

        var msg = new SendGridMessage
        {
            From = fromAddress,
            Subject = subject,
            HtmlContent = htmlContent,
            PlainTextContent = plainTextContent
        };
        msg.AddTo(toAddress);

        return msg;
    }
}
