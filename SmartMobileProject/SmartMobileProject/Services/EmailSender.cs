using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartMobileProject.Services
{
    class EmailSender
    {
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                Console.WriteLine($"Email is not supported on this device {fbsEx}");
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                Console.WriteLine($"Some Other Exeption In Email occured {ex}");
            }
        }
    }
}
