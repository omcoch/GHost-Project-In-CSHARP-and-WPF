using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BE
{
    public class Cookies
    {
        private static Stack<string> prevWindow = new Stack<string>(); // Stack for history navigation
        public static int LoginUserKey { get; set; } // Holds the login-key of the current host
        public static string PrevWindow
        {
            get {
                prevWindow.Pop();
                return prevWindow.Peek();
            }
            set {
                if (prevWindow.Count == 0 || value != prevWindow.Peek())
                    prevWindow.Push(value);
            }
        }

        /// <summary>
        /// Check if the stack of history is empty (except from the main-window
        /// </summary>
        /// <returns>True if the only window in the history is the Main Window, else False</returns>
        public static bool NoHistory()
        {
            return  prevWindow.Count <= 1;
        }

        public static void LogOut()
        {
            prevWindow.Clear();
            LoginUserKey = 0;
        }
    }
}
