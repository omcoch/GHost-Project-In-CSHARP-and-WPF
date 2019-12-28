using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        private static int guestRequestSerialKey = 10000000;
        private static int orderSerialKey = 10000000;
        private static int hostingUnitSerialKey = 10000000;
        
        public static int GuestRequestSerialKey { get => guestRequestSerialKey++; }
        public static int OrderSerialKey { get => orderSerialKey++; }
        public static int HostingUnitSerialKey { get => hostingUnitSerialKey++; }
    }
}
