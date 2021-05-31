using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task SendMail(SmtpOption smtp, MailOption mail, IDictionary<string, string> variables = null, bool singleReceiver = false)
        {
            if (smtp is null || mail is null)
                return;

            var receivers = await mail.GetReceivers().ConfigureAwait(false);
            if (!receivers.Any())
                return;

            var stopwatch = new Stopwatch();

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

                if (variables != null && variables.Any())
                    mailMessage.Body = mailMessage.Body.ReplaceVariables(variables);

                using var smtpClient = new SmtpClient(smtp.Host, smtp.Port);
                smtpClient.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                smtpClient.EnableSsl = smtp.EnableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                stopwatch.Start();

                if (singleReceiver)
                {
                    var counter = 1;
                    foreach (var mailAddress in receivers)
                    {
                        try
                        {
                            ColorConsole.WriteInfo($"⏳ Sending mail to {mailAddress} ({counter})", true);
                            
                            mailMessage.To.Clear();
                            mailMessage.To.Add(mailAddress);

                            await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            ColorConsole.WriteError($"Failed send mail to {mailAddress} {Environment.NewLine}{ex.Message}", true);
                        }
                        finally
                        {
                            counter++;
                        }
                    }
                }
                else
                {
                    ColorConsole.WriteInfo($"⏳ Sending mail to {string.Join(", ", receivers)}", true);
                    
                    foreach (var mailAddress in receivers)
                        mailMessage.To.Add(mailAddress);

                    await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);
                }

                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                ColorConsole.WriteError($"Oops.. Something went wrong! {Environment.NewLine}{ex.Message}", true);
            }
            finally
            {
                stopwatch.Stop();
                ColorConsole.WriteInfo("⌛️ Total elapsed time: " + stopwatch.Elapsed, true);
            }
        }
    }
}
