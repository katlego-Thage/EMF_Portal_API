using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API
{
    public class Email
    {
        public System.Guid EmailId { get; set; }
        public string EmailTo { get; set; }
        public string EmailToFullName { get; set; }

        public string EmailCC { get; set; }

        public string EmailCCFullName { get; set; }

        public string EmailFrom { get; set; }
        public string EmailFromFullName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool EmailStatus { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsHtml { get; set; } = false;
    }
}
