using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SendMail.Options
{
    public class MailOption
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Receiver { get; set; }

        public async Task<IEnumerable<string>> GetReceivers()
        {
            var receiverPath = Path.Combine(Directory.GetCurrentDirectory(), Receiver);
            var mails = new List<string>();
            var mail = default(string);

            using var sr = new StreamReader(receiverPath);
            while ((mail = await sr.ReadLineAsync()) != null && !string.IsNullOrWhiteSpace(mail) && !mail.StartsWith('/'))
                mails.Add(mail);

            return mails;
        }
    }
}