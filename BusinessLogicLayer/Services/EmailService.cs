using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class EmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(
             "gaurav134mishra@gmail.com", 
             "BridgeLabzSolutions"                  
         );
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    "gaurav134mishra@gmail.com",
                    "cttmyscvtcebytue"
                ),
                EnableSsl = true
            };

            smtp.Send(mail);
        }
    }

}
