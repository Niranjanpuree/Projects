using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using IEmailSender = Northwind.Core.Interfaces.IEmailSender;

namespace Northwind.Core.Services
{
    public class EmailService : IEmailSender
    {
        public string FromMailAddress { get; set; }
        public string FromMailAddressTitle { get; set; }
        public string ToMailAddressTitle { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPortNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mode { get; set; }
        public string ToEmailAddress { get; set; }
        private string[] modeArray = { "auto", "off", "dev" };
        private string autoMode = "auto";
        private string offMode = "off";
        private string devMode = "dev";
        public EmailService(string fromMailAddress, string fromMailAddressTitle,
                            string toMailAddressTitle, string smtpServer,
                            string smtpPortNumber, string username, string password,string mode, string toEmailAddress)
        {
            FromMailAddress = fromMailAddress;
            FromMailAddressTitle = fromMailAddressTitle;
            ToMailAddressTitle = toMailAddressTitle;
            SmtpServer = smtpServer;
            SmtpPortNumber = smtpPortNumber;
            Username = username;
            Password = password;
            Mode = mode;
            ToEmailAddress = toEmailAddress;
        }
        public async Task<Boolean> SendEmailAsync(string emailAddresses, string recipientName, string subject, string message)
        {
            try
            {
                var emailAddressToSend = string.Empty;
                if (!string.IsNullOrWhiteSpace(Mode) && Mode.ToLower() == offMode)
                    return true;
                else if (!string.IsNullOrWhiteSpace(Mode) && Mode.ToLower() == devMode)
                    emailAddressToSend = ToEmailAddress;
                else if (!string.IsNullOrWhiteSpace(Mode) && Mode.ToLower() == autoMode)
                    emailAddressToSend = emailAddresses;
                else
                    return false;

                if (!string.IsNullOrWhiteSpace(emailAddressToSend))
                {
                    string FromAddress = FromMailAddress;
                    string FromAdressTitle = FromMailAddressTitle;
                    //To Address  
                    string ToAdressTitle = ToMailAddressTitle;
                    string Subject = subject;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress
                                            (FromAdressTitle,
                                             FromAddress
                                             ));
                    foreach (var mail in emailAddressToSend.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mimeMessage.To.Add(new MailboxAddress(mail) { Address = mail, Name = recipientName });
                    }

                    mimeMessage.Subject = Subject; //Subject
                    var multipart = new Multipart("mixed");
                    multipart.Add(new TextPart("html") { Text = message });
                    mimeMessage.Body = multipart;
                    using (var client = new SmtpClient())
                    {
                        client.SslProtocols = SslProtocols.None;
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.Connect(SmtpServer, Convert.ToInt32(SmtpPortNumber), SecureSocketOptions.Auto);
                        client.Authenticate(
                            Username,
                            Password
                            );
                        await client.SendAsync(mimeMessage);
                        await client.DisconnectAsync(true);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
