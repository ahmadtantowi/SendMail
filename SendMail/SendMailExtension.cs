using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SendMail
{
    public static class SendMailExtension
    {
        public static string ReplaceVariables(this string source, IDictionary<string, string> variables, string tokenizeStart = "{{", string tokenizeEnd = "}}")
        {
            var regex = new Regex(@$"\{tokenizeStart}(\w+)\{tokenizeEnd}", RegexOptions.Compiled);

            return regex.Replace(source, match => variables[match.Groups[1].Value]);
        }
    }
}