using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BE;

namespace Utilities
{
    public class Tools
    {
        public static bool SendMail(MailMessage message)
        {
            message.From = new MailAddress(Configuration.SiteName);
            // Smtp
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Credentials = new System.Net.NetworkCredential(Configuration.AdminMailAddress.ToString(), "myGmailPassword"), // todo: להכניס סיסמה אמיתית
                EnableSsl = true
            };

            try
            {
                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ValidateString(string str)
        {
            return !String.IsNullOrEmpty(str) && Regex.IsMatch(str, "^[א-תa-zA-Z ]+$");
        }
        public static bool ValidateNumber(string str,int max=int.MaxValue)
        {
            try
            {
                int n = int.Parse(str);
                return n <= max && n >= 0;
            }
            catch
            {
                return false;
            }
        }
        public static bool ValidatePhoneNumber(string str)
        {
            return !String.IsNullOrEmpty(str) && Regex.IsMatch(str, "^0")&&int.TryParse(str,out int x);
        }
    
        public static bool ValidateEmailAddress(string text)
        {
            return !String.IsNullOrEmpty(text) && new EmailAddressAttribute().IsValid(text);
        }
    }
}
