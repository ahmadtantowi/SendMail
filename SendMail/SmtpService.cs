using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendMail.Helpers;
using SendMail.Options;

namespace SendMail
{
    public class SmtpService
    {
        public async Task SendMail(SmtpOption smtp, MailOption mail, bool singleReceiver = false)
        {
            if (smtp is null || mail is null)
                return;

            var receivers = await mail.GetReceivers().ConfigureAwait(false);
            if (!receivers.Any())
                return;

            try
            {
                using var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtp.UserName);
                mailMessage.Subject = mail.Subject;
                mailMessage.IsBodyHtml = mail.IsBodyHtml;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                
                if (mail.IsBodyHtml)
                {
                    using var sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), mail.Body));
                    mailMessage.Body = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
                else
                {
                    mailMessage.Body = mail.Body;
                }

                using var smtpClient = new SmtpClient(smtp.Host, smtp.Port);
                smtpClient.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                smtpClient.EnableSsl = smtp.EnableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (singleReceiver)
                {
                    foreach (var mailAddress in receivers)
                    {
                        mailMessage.To.Clear();
                        mailMessage.To.Add(mailAddress);
                        
                        ColorConsole.WriteInfo($"⏳ Sending mail to {mailAddress}", true);
                        await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                    }
                }
                else
                {
                    foreach (var mailAddress in receivers)
                        mailMessage.To.Add(mailAddress);
                    
                    ColorConsole.WriteInfo($"⏳ Sending mail to {string.Join(", ", receivers)}", true);
                    await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                ColorConsole.WriteError($"Oops.. Something went wrong! {Environment.NewLine}{ex.Message}", true);
            }
        }
    }
}