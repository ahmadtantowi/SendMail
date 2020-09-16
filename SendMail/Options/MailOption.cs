using System;

namespace SendMail.Options
{
    public class MailOption
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}