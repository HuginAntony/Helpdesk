using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Helpdesk.Core.Helpers
{
    public class EmailHelper
    {
        public bool SendEmail(List<string> recipients, string mailSubject, string content)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                var fromAddress = new MailAddress("info@helpdesk.com", "Helpdesk Info", Encoding.UTF8);
                
                var message = new MailMessage()
                {
                    From = fromAddress,
                    Body = content,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Subject = mailSubject,
                    SubjectEncoding = Encoding.UTF8
                };

                foreach (var recipient in recipients)
                {
                    message.To.Add(recipient);    
                }
                
                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}