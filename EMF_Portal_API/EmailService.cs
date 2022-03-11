using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EMF_Portal_API
{
    public class EmailService
    {
        public async Task SendMail(Email email)
        {
            MailMessage message = new MailMessage();
            message.Subject = email.EmailSubject;
            message.Body = email.EmailBody;
            if (email.EmailFrom == "" || email.EmailFrom == null)
            {

                message.From = new MailAddress("thagek@outlook.com", "Elite Minds Foundation");
            }
            else
            {
                message.From = new MailAddress(email.EmailFrom, "");
            }
            message.To.Add(new MailAddress(email.EmailTo, email.EmailFromFullName));

            if (email.EmailCC != null)
            {
                message.CC.Add(new MailAddress(email.EmailCC, email.EmailCCFullName));
            }

            if (email.IsHtml)
            {
                message.IsBodyHtml = true;
            }

            string smtpUrl = "smtp.office365.com";
            string smtpPort = "587";
            string smtpUsername = "thagek@outlook.com";
            string smtpPassword = "Kat@211a";

            bool enableSSL = true;
            SmtpClient smtpClient = new SmtpClient(smtpUrl, Convert.ToInt32(smtpPort));
            if (smtpUsername != "" && smtpUsername != null)
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            }
            else
            {
                smtpClient.UseDefaultCredentials = false;
            }
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = enableSSL;
            smtpClient.TargetName = "STARTTLS/smtp.office365.com";
            try
            {

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                //do nothing //exception is blank it fails
                //since store in the db have it there
                //logging.AddEventLog();
            }

        }
    }
}
