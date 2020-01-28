using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        public int HostingUnitKey { get; set;}
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }
        /// <summary> סטטוס ההזמנה </summary>
        public OrderStatus Status { get; set; }
        /// <summary> תאריך יצירת ההזמנה </summary>
        public DateTime CreateDate { get; set; } 
        /// <summary> תאריך משלוח המייל ללקוח </summary>
        public DateTime OrderDate { get; set; } 

        public override string ToString()
        {
            return "מספר הזמנה:"+
                "\nמזהה לקוח: " + HostingUnitKey+" מזהה יחידת אירוח: "+ GuestRequestKey+
                "\nסטטוס: " + Status
                + "\nתאריך יצירה: " + CreateDate
                + "\nתאריך שליחת מייל: " + OrderDate;
        }
    }
}
