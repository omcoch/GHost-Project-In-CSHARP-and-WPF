using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Tools
    {
        
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
            for (int j = 0; j < columns; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    var test = arr[i, j];
                    arrFlattened[i + j * rows] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    arrExpanded[i, j] = arr[i + j * rows];
                }
            }
            return arrExpanded;
        }
    }
}
