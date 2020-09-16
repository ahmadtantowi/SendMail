using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using SendMail.Options;

namespace SendMail
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var smtpOption = builder.GetSection(nameof(SmtpOption)).Get<SmtpOption>();
            var mailOption = builder.GetSection(nameof(MailOption)).Get<MailOption>();
        }
    }
}
