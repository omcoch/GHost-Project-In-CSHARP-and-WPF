using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        private static int guestRequestSerialKey = 10000000;//מספר מזהה רץ עבור בקשת לקוח
        private static int orderSerialKey = 10000000;//מספר מזהה רץ עבור הזמנה
        private static int hostingUnitSerialKey = 10000000;//מספר מזהה רץ עבור יחדית אירוח
        public const int fee = 10;//עמלה
        public static int GuestRequestSerialKey { get => guestRequestSerialKey++; }
        public static int OrderSerialKey { get => orderSerialKey++; }
        public static int HostingUnitSerialKey { get => hostingUnitSerialKey++; }
    }
}
