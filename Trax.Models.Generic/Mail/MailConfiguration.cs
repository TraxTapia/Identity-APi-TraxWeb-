using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Mail
{
    public class MailConfiguration
    {
        public bool SMTPEnableSSL { get; set; }
        public int SMTPTimeOut { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPFromMail { get; set; }
    }
}
