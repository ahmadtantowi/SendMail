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
    }
}