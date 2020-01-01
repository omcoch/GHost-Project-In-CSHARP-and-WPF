﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        public int HostingUnitKey { get; set;}
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }

        public OrderStatus Status { get; set; }
        /// <summary> תאריך יצירת ההזמנה </summary>
        public DateTime CreateDate { get; set; } 
        /// <summary> תאריך משלוח המייל ללקוח </summary>
        public DateTime OrderDate { get; set; } 
        public override string ToString()
        {
            return base.ToString();
        }
    }
}