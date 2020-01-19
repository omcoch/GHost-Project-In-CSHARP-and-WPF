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
        public static bool validateString(string str)
        {
            return !String.IsNullOrEmpty(str) && Regex.IsMatch(str, "^[א-תa-zA-Z]+$");
        }
        public static bool ValidateNumber(string str)
        {
            try
            {
                int n = int.Parse(str);
                return n < 100 && n >= 0;
            }
            catch
            {
                return false;
            }
        }
        public static bool ValidateEmailAddress(string text)
        {
            return !String.IsNullOrEmpty(text) && new EmailAddressAttribute().IsValid(text);
        }
    }
}
