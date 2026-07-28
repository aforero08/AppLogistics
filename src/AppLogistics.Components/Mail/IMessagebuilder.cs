using SendGrid.Helpers.Mail;

namespace AppLogistics.Components.Mail;

public interface IMessagebuilder
{
    SendGridMessage BuildMessageFromAdmin(string recipientEmail, string recipientName, string subject,
        string htmlContent, string plainTextContent);

    SendGridMessage BuildMessage(string senderEmail, string senderName, string recipientEmail,
        string recipientName, string subject, string htmlContent, string plainTextContent);
}
