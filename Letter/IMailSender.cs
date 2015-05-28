using System;
using System.Net.Mail;

namespace Letter
{
    public interface IMailSender : IDisposable
    {
        void Send(string fromAddress, string toAddress, string subject, string body);

        void Send(MailAddress fromAddress, MailAddress toAddress, string subject, string body);

        void Send(MailMessage mailMessage);
    }
}