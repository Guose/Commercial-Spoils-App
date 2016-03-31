using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Commercial_Spoils_App
{
    class Email
    {
        public void SendMail()
        {
            string to = "guose79@gmail.com";
            string from = "guose79@gmail.com";

            MailMessage message = new MailMessage(from, to);

            message.Body = "";
        }
    }
}
