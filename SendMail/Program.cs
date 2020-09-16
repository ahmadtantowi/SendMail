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

            var smtpOption = builder.GetSection(nameof(SmtpOption)).Get<SmtpOption>();
            var mailOption = builder.GetSection(nameof(MailOption)).Get<MailOption>();

            var smtpService = new SmtpService();
            await smtpService.SendMail(smtpOption, mailOption, true);
            Console.WriteLine("End of application. Goodbye.");
        }
    }
}
