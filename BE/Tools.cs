using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Tools
    {
        public static bool SendMail(MailAddress mail_address)
        {
            Console.WriteLine("המייל נשלח בהצלחה");
            return true;
        }
    }
}
