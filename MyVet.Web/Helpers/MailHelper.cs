using MailKit.Net.Proxy;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;

namespace MyVet.Web.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string to, string subject, string body)
        {
            try
            {
                var from = _configuration["Mail:From"];
                var smtp = _configuration["Mail:Smtp"];
                var port = _configuration["Mail:Port"];
                var password = _configuration["Mail:Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from));
                message.To.Add(new MailboxAddress(to));
                message.Subject = subject;
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    //var credentials = new NetworkCredential("FABIDOM\\soportet", "S@p@rt3T9");
                    //client.ProxyClient = new Socks5Client("isaserver.lafabril.com.do", 8080, credentials);
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}