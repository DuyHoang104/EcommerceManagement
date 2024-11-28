using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceManagement.Crosscutting.Emails.Settings
{
    public class SmtpConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
