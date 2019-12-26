using System;
using System.Collections.Generic;
using BE;
namespace DS
{
    public class DataSource
    {
        public static List<GuestRequest> GuestRequests = new List<GuestRequest>() { new GuestRequest() { PrivateName = "vd" } };
        public static List<HostingUnit> HostingUnits;
        public static List<Order> Orders;
    }
}
