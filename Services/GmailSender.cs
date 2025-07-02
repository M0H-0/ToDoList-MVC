using System.Net;
using System.Net.Mail;

namespace ToDoList.Services;

public class GmailSender
{
    private readonly string _from;
    private readonly string _password;

    public GmailSender(IConfiguration config)
    {
        _from = config["Email:Gmail"]!;
        _password = config["Email:Password"]!;
    }
    public void SendEmail(string to, string subject, string htmlMessage)
    {
        var message = new MailMessage(_from, to, subject, htmlMessage) { IsBodyHtml = true };
        new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_from, _password),
            EnableSsl = true
        }.Send(message);
    }
}