using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CrowdCover.Web.Tests.Integration.Email
{
    public class EmailSenderTests
    {
        private IEmailSender _emailSender;

        [SetUp]
        public void Setup()
        {
            // Initialize the EmailSender with Gmail SMTP credentials
            _emailSender = new EmailSender(
                smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUser: "team@crowdcover.live",   // your email
                smtpPass: "jsio avks iouy wyky"         // your password
                                                        //smtpUser: "emailnadz@gmail.com",
                                                        //smtpPass:"Diagonal23!!"
            );

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        [Test]
        public async Task SendEmail()
        {
            // Test email recipient, subject, and body
            var recipientEmail = "emailnadz@gmail.com"; // Change this to a valid recipient email
            var subject = "Test Email from Integration Test";
            var htmlMessage = "<p>This is a test email sent during a live integration test.</p>";

            // Act: Send the email
            await _emailSender.SendEmailAsync(recipientEmail, subject, htmlMessage);

            // No direct assertion possible on email sending, but if no exception is thrown, the test passes
            Assert.Pass("Email sent successfully without exceptions.");
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of resources if necessary
        }
    }
}
