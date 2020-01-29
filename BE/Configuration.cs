using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        private static int guestRequestSerialKey = 10000000;//מספר מזהה רץ עבור בקשת לקוח
        private static int orderSerialKey = 10000000;//מספר מזהה רץ עבור הזמנה
        private static int hostingUnitSerialKey = 10000000;//מספר מזהה רץ עבור יחדית אירוח
        private static bool s_banksXmlFinish = false; // מורה על סיום טעינת קובץ הבנקים
        public static readonly MailAddress AdminMailAddress = new MailAddress("readtora@gmail.com"); // כתובת מייל של בעל האתר
        public static string SiteName { get => "G-Host"; } // שם האפליקציה

        public const int FEE = 10;//עמלה
        public static int GenerateGuestRequestSerialKey { get => guestRequestSerialKey++; }
        public static int GenerateOrderSerialKey { get => orderSerialKey++; }
        public static int GenerateHostingUnitSerialKey { get => hostingUnitSerialKey++; }
        public static bool BanksXmlFinish { get => s_banksXmlFinish; set => s_banksXmlFinish = value; }
    }
}
