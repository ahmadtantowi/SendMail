using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendMail.Helpers;
using SendMail.Options;

namespace SendMail
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ColorConsole.WriteWrappedHeader("📧 Send Mail Console Application 📧");
            ColorConsole.WriteInfo("🏃 Starting application...", true);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var smtpService = new SmtpService();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                ColorConsole.WriteEmbeddedColorLine("[[darkcyan]1[/darkcyan]] 📤 Send Email");
                ColorConsole.WriteEmbeddedColorLine("[[darkcyan]0[/darkcyan]] 🚪 Exit");
                
                SelectMenu:
                ColorConsole.WriteInfo("Select number of Menu: ");
                var readLine = Console.ReadLine();

                if (readLine == "1")
                {
                    var smtpOption = builder.GetSection(nameof(SmtpOption)).Get<SmtpOption>();
                    var mailOptions = builder.GetSection(nameof(MailOption)).Get<MailOption[]>() ?? Array.Empty<MailOption>();
                    var variables = new Dictionary<string, string>();
                    
                    var filesHostUrl = builder.GetValue<string>("FilesHostUrl");
                    if (!string.IsNullOrEmpty(filesHostUrl))
                        variables.Add("FilesHostUrl", filesHostUrl);
                    
                    if (mailOptions.Length != 0)
                    {
                        while (true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Available Email Template:");
                            for (int i = 0; i < mailOptions.Length; i++)
                                ColorConsole.WriteEmbeddedColorLine($"[[darkcyan]{i + 1}[/darkcyan]] 📧 {mailOptions[i].Subject}");
                            ColorConsole.WriteEmbeddedColorLine("[[darkcyan]0[/darkcyan]] ❌ Cancel");
                            
                            SelectTemplate:
                            ColorConsole.WriteInfo("Select number of Email Template: ");
                            readLine = Console.ReadLine();

                            if (int.TryParse(readLine, out var selectedTemplate) && selectedTemplate >= 0 && selectedTemplate <= mailOptions.Length)
                            {
                                if (selectedTemplate != 0)
                                {
                                    await smtpService.SendMail(smtpOption, mailOptions[selectedTemplate - 1], variables, true);
                                    ColorConsole.WriteSuccess("✅ Done!", true);
                                }
                                break;
                            }
                            else
                            {
                                ColorConsole.WriteWarning("You choose wrong menu. Try again!", true);
                                goto SelectTemplate;
                            }
                        }
                    }
                    else
                    {
                        ColorConsole.WriteError("There is no email template available. Please provide into appsettings.json!", true);
                    }
                }
                else if (readLine == "0")
                {
                    break;
                }
                else
                {
                    ColorConsole.WriteWarning("You choose wrong menu. Try again!", true);
                    goto SelectMenu;
                }
            }

            ColorConsole.WriteSuccess("End of application. Goodbye 👋👋👋", true);
        }
    }
}
