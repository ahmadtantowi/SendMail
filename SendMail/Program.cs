using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendMail.Options;

namespace SendMail
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting application...");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var smtpService = new SmtpService();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                Console.WriteLine("[1] Send Email");
                Console.WriteLine("[0] Exit");
                
                SelectMenu:
                Console.Write("Select number of Menu: ");
                var readLine = Console.ReadLine();

                if (readLine == "1")
                {
                    var smtpOption = builder.GetSection(nameof(SmtpOption)).Get<SmtpOption>();
                    var mailOptions = builder.GetSection(nameof(MailOption)).Get<MailOption[]>() ?? Array.Empty<MailOption>();
                    
                    if (mailOptions.Length != 0)
                    {
                        while (true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Available Email Template:");
                            for (int i = 0; i < mailOptions.Length; i++)
                                Console.WriteLine($"[{i + 1}] {mailOptions[i].Subject}");
                            Console.WriteLine("[0] Cancel");
                            
                            SelectTemplate:
                            Console.Write("Select number of Email Template: ");
                            readLine = Console.ReadLine();

                            if (int.TryParse(readLine, out var selectedTemplate) && selectedTemplate >= 0 && selectedTemplate <= mailOptions.Length)
                            {
                                if (selectedTemplate != 0)
                                {
                                    await smtpService.SendMail(smtpOption, mailOptions[selectedTemplate - 1], true);
                                    Console.WriteLine("Done!");
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("You choose wrong menu. Try again!");
                                goto SelectTemplate;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("There is no email template available. Please provide into appsettings.json!");
                    }
                }
                else if (readLine == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You choose wrong menu. Try again!");
                    goto SelectMenu;
                }
            }

            Console.WriteLine("End of application. Goodbye.");
        }
    }
}
