using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SendMail.Options;

namespace SendMail
{
    public static class SendMailExtension
    {
        public static string ReplaceVariables(this string source, IDictionary<string, string> variables, string tokenizeStart = "{{", string tokenizeEnd = "}}")
        {
            var regex = new Regex(@$"\{tokenizeStart}(\w+)\{tokenizeEnd}", RegexOptions.Compiled);

            return regex.Replace(source, match => variables[match.Groups[1].Value]);
        }

        public static async Task<IEnumerable<string>> GetReceivers(this MailOption mailOption)
        {
            var receiverPath = Path.Combine(Directory.GetCurrentDirectory(), mailOption?.Receiver ?? throw new ArgumentException("Receiver text file not provided."));
            var mails = new List<string>();
            var mail = default(string);

            using var sr = new StreamReader(receiverPath);
            while ((mail = await sr.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrWhiteSpace(mail) && !mail.StartsWith('/'))
                    mails.Add(mail);
            }

            return mails;
        }
    }
}