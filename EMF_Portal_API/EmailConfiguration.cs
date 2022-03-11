using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API
{
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SiteBaseUrl { get; set; }
        public bool SmtpEnableSSL { get; set; }
        public string DefaultEmailFromAddress { get; set; }
        public string DefaultEmailFromDisplayName { get; set; }
    }
}
