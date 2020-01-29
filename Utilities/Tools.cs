using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Utilities
{
    public static class Tools
    {
        /// <summary>
        /// שליחת מייל
        /// </summary>
        /// <param name="message">ההודעה לשליחה</param>
        /// <param name="siteName">שם האתר (השולח)</param>
        /// <param name="adminMailAddress">כתובת מייל השולח (בעל האתר בלבד)</param>
        /// <returns></returns>
        public static bool SendMail(MailMessage message, string siteName, string from)
        {
            // Smtp
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(from, "g-host12");
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                smtp.Send(message);
                return true;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "שגיאה בשליחת המייל");
            }
            return false;
        }

        public static bool ValidateString(string str)
        {
            return !String.IsNullOrEmpty(str) && Regex.IsMatch(str, "^[א-תa-zA-Z ]+$");
        }
        public static bool ValidateNumber(string str, int max = int.MaxValue, bool isDouble=false)
        {
            try
            {
                var n = isDouble ? int.Parse(str) :
                                double.Parse(str);
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

        public static T[] Flatten<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            T[] arrFlattened = new T[rows * columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var test = arr[i, j];
                    arrFlattened[i * rows + j] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    arrExpanded[i, j] = arr[i * rows + j];
                }
            }
            return arrExpanded;
        }
    }
}
