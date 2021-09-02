using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Email
{
    public class EmailGonderici : IEmailSender
    {
        public EmailOptions Options { get; set; }

        public EmailGonderici(IOptions<EmailOptions> emailOptions)
        {
            Options = emailOptions.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var mesaj = new SendGridMessage()
            {
                From = new EmailAddress("batu_6407@hotmail.com.tr","Razor Page egitim"),//send gridde kayıt olduğun email ve başlık
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };

            mesaj.AddTo(new EmailAddress(email));

            try
            {
                return client.SendEmailAsync(mesaj);
            }
            catch (Exception)
            {

               
            }

            return null;

        }
    }
}
