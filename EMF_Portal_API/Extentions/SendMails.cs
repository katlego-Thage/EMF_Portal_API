using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Extentions
{
    public class SendMails
    {
        public async Task SendConfirmationAsync(string email, string link, string password)
        {
            var emailService = new EmailService();
            var emailMsg = new Email
            {
                EmailTo = email,
                EmailToFullName = email,
                EmailSubject = "Account created",
                IsHtml = true,
                EmailBody =
                                   //"       **********This is an automated email message**********<br />"
                                   "**********************<br />"
                                   + "Hi " + email
                                   + "<br />**********************"
                                   + "<br /><br />"
                                   + "A New Account has been created for you in the Elite Minds Foundation System. Please click" + string.Format(" <a href='{0}'>here</a>", link) + " to confirm your account"
                                   + "<br />"
                                   + "ACCOUNT DETAILS<br />"
                                   + "****************<br />"
                                   + "Username: " + email
                                   + "<br />Password: " + password
                                   + "<br /><br />Thanks "
                                    + "<br />The Elite Minds Foundation Team"
            };
            await emailService.SendMail(emailMsg);
        }

        public async Task PasswordResetAsync(string email, string callbackUrl)
        {
            var emailService = new EmailService();
            var emailMsg = new Email
            {
                EmailTo = email,
                EmailToFullName = email,
                EmailSubject = "Reset Password",
                IsHtml = true,
                EmailBody = "**************************<br />"
                                   + "Hi " + email
                                   + "<br />**************************<br /><br />"
                                   + "You recently requested to reset your password for you Elite Minds Foundation account.<br />"
                                   + "Use the button below to reset it.<br /><br />"
                                   + string.Format(" <a href='{0}'>RESET PASSWORD</a>", callbackUrl)
                                   + "<br /><br />If you did not request a password reset then ignore this message.<br /><br />"
                                   + "Thanks <br />"
                                    + "The Elite Minds Foundation Team"
            };
            await emailService.SendMail(emailMsg);
        }
    }
}
