﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IDAL
    {

        void AddGuestRequest(GuestRequest guestRequest);
        void UpdateGuestRequest(GuestRequest guestRequest);

        void AddHostingUnit(HostingUnit hostingUnit);
        void RemoveHostingUnit(int key);
        void UpdateHostingUnit(HostingUnit hostingUnit);

        void AddOrder(Order order);
        void UpdateOrder(Order order);

        List<GuestRequest> GetGuestRequests();
        List<HostingUnit> GetHostingUnits();
        List<Order> GetOrders();

        List<BankBranch> GetBankBranches();
    }

}
