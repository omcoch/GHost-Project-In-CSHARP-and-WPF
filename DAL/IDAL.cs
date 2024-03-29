﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IDAL
    {

        int AddGuestRequest(GuestRequest guestRequest);
        void UpdateGuestRequest(GuestRequest guestRequest);

        string AddHost(Host host);
        void UpdateHost(Host host);

        int AddHostingUnit(HostingUnit hostingUnit);
        void RemoveHostingUnit(int key);
        void UpdateHostingUnit(HostingUnit hostingUnit);

        int AddOrder(Order order);
        void UpdateOrder(Order order);

        List<GuestRequest> GetGuestRequests();
        List<HostingUnit> GetHostingUnits();
        List<Order> GetOrders();
        List<Host> GetHosts();


        List<BankBranch> GetBankBranches();
    }

}
