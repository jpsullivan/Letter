using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Letter
{
    public class MailSender : IMailSender
    {
        private readonly ISmtpClient _smtpClient;

        public MailSender()
            : this(new SmtpClientWrapper(new SmtpClient()), null)
        {
        }

        public MailSender(MailSenderConfiguration configuration)
            : this(new SmtpClientWrapper(new SmtpClient()), configuration)
        {
        }

        public MailSender(SmtpClient smtpClient)
            : this(new SmtpClientWrapper(smtpClient), null)
        {
        }

        internal MailSender(ISmtpClient smtpClient, MailSenderConfiguration configuration)
        {
            if (smtpClient == null)
                throw new ArgumentNullException("smtpClient");

            if (configuration != null)
                ConfigureSmtpClient(smtpClient, configuration);

            _smtpClient = smtpClient;
        }

        internal static void ConfigureSmtpClient(ISmtpClient smtpClient, MailSenderConfiguration configuration)
        {
            if (configuration.Host != null)
                smtpClient.Host = configuration.Host;
            if (configuration.Port.HasValue)
                smtpClient.Port = configuration.Port.Value;
            if (configuration.EnableSsl.HasValue)
                smtpClient.EnableSsl = configuration.EnableSsl.Value;
            if (configuration.DeliveryMethod.HasValue)
                smtpClient.DeliveryMethod = configuration.DeliveryMethod.Value;
            if (configuration.UseDefaultCredentials.HasValue)
                smtpClient.UseDefaultCredentials = configuration.UseDefaultCredentials.Value;
            if (configuration.Credentials != null)
                smtpClient.Credentials = configuration.Credentials;
            if (configuration.PickupDirectoryLocation != null)
                smtpClient.PickupDirectoryLocation = configuration.PickupDirectoryLocation;
        }

        public void Send(string fromAddress, string toAddress, string subject, string body)
        {
            Send(new MailAddress(fromAddress), new MailAddress(toAddress), subject, body);
        }

        public void Send(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        {
            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            Send(mailMessage);
        }

        public void Send(MailMessage mailMessage)
        {
            if (_smtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory &&
                !Directory.Exists(_smtpClient.PickupDirectoryLocation))
            {
                Directory.CreateDirectory(_smtpClient.PickupDirectoryLocation);
            }

            var pm = new PreMailer.Net.PreMailer(mailMessage.Body);
            var htmlBody = pm.MoveCssInline();

//            string markdownBody = mailMessage.Body;
//            string htmlBody = new Markdown().Transform(markdownBody);

//            AlternateView textView = AlternateView.CreateAlternateViewFromString(
//                markdownBody, 
//                null, 
//                MediaTypeNames.Text.Plain);
//            mailMessage.AlternateViews.Add(textView);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                htmlBody.Html,
                null,
                MediaTypeNames.Text.Html);
            mailMessage.AlternateViews.Add(htmlView);

            _smtpClient.Send(mailMessage);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }
}