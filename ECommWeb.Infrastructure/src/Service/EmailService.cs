using System.Net;
using System.Net.Mail;
using System.Text;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

namespace ECommWeb.Infrastructure.src.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void SendEmail(string email, string subject, string body)
    {
        var fromAddress = _configuration["MailCredentials:FromMail"];
        var frompassWord = _configuration["MailCredentials:MailPassword"];

        var client = new SmtpClient("smtp.ethereal.email", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress, frompassWord),
        };

        var name = email.Substring(0, email.IndexOf('@'));
        var userName = name[0].ToString().ToUpper() + name.Substring(1);

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(fromAddress);
        mailMessage.To.Add(email);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        StringBuilder mailBody = new StringBuilder();
        mailBody.AppendFormat("<p>Hello {0}</p>", userName);
        mailBody.AppendFormat("<br />");
        mailBody.AppendFormat(body);
        mailBody.AppendFormat("<p>Thank you For Ordering</p>");
        mailBody.AppendFormat("<br />");
        mailBody.AppendFormat("<p>Regards</p>");
        mailBody.AppendFormat("<p>Team ModaMorph</p>");
        mailMessage.Body = mailBody.ToString();

        client.Send(mailMessage);
    }

}
