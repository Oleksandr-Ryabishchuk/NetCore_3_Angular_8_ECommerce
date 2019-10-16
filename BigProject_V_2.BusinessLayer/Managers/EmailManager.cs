using BigProject_V_2.BusinessLayer.Interfaces;
using BigProject_V_2.DataAccessLayer.Dtos;
using BigProject_V_2.DataAccessLayer.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigProject_V_2.BusinessLayer.Managers
{
    public class EmailManager: IEmailManager
    {
        private readonly ApplicationSettings _applicationSettings;
        public EmailManager(IOptions<ApplicationSettings> applicationSettings)
        {
            _applicationSettings = applicationSettings.Value;
        }

        public async Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var apiKey = _applicationSettings.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("spicyco88@gmail.com", "SpicyCo.com"); // ryabishcuk2@gmail.com
            var subject = emailSubject;
            var to = new EmailAddress(userEmail, "Example User");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return new SendEmailResponse();
        }
    }
}
