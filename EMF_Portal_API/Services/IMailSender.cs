using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Services
{
    public interface IMailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
