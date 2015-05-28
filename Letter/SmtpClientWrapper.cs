using System.Net;
using System.Net.Mail;

namespace Letter
{
    internal class SmtpClientWrapper : ISmtpClient
    {
        readonly SmtpClient _smtpClient;

        public SmtpClientWrapper(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public ICredentialsByHost Credentials
        {
            get { return _smtpClient.Credentials; }
            set { _smtpClient.Credentials = value; }
        }

        public SmtpDeliveryMethod DeliveryMethod
        {
            get { return _smtpClient.DeliveryMethod; }
            set { _smtpClient.DeliveryMethod = value; }
        }

        public bool EnableSsl
        {
            get { return _smtpClient.EnableSsl; }
            set { _smtpClient.EnableSsl = value; }
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

        public string Host
        {
            get { return _smtpClient.Host; }
            set { _smtpClient.Host = value; }
        }

        public string PickupDirectoryLocation
        {
            get { return _smtpClient.PickupDirectoryLocation; }
            set { _smtpClient.PickupDirectoryLocation = value; }
        }

        public int Port
        {
            get { return _smtpClient.Port; }
            set { _smtpClient.Port = value; }
        }

        public void Send(MailMessage message)
        {
            _smtpClient.Send(message);
        }

        public bool UseDefaultCredentials
        {
            get { return _smtpClient.UseDefaultCredentials; }
            set { _smtpClient.UseDefaultCredentials = value; }
        }
    }
}