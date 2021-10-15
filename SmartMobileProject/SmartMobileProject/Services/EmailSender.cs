using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.Services
{
    class EmailSender
    {
        public static async Task SendEmail(string subject, string body, List<string> recipients)
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
        public static async Task SendFileEmail(string subject, string body, List<string> recipients, string printPage)
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

                var fn = "Attachment.pdf";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                CreatePrintView print = new CreatePrintView();
                File.WriteAllText(file,printPage);
                
                message.Attachments.Add(new EmailAttachment(file));

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
