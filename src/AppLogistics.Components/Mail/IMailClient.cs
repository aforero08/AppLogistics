using System.Threading.Tasks;

namespace AppLogistics.Components.Mail;

public interface IMailClient
{
    Task SendFromAdmin(string recipientEmail, string recipientName, string subject, string body);
}
