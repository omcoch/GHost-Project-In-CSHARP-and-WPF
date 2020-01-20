﻿using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        int AddGuestRequest(GuestRequest guestRequest);
        void UpdateGuestRequest(GuestRequest guestRequest);

        int AddHost(Host host);
        void UpdateHost(Host host);

        int AddHostingUnit(HostingUnit hostingUnit);
        void RemoveHostingUnit(int key);
        void UpdateHostingUnit(HostingUnit hostingUnit);

        int AddOrder(Order order);
        void UpdateOrder(Order order);

        List<HostingUnit> GetAvailableHostingUnits(DateTime date, int days);
        int TimeDistance(DateTime first, DateTime last = default(DateTime));

        List<Order> GetOrdersBefore(int days);
        List<GuestRequest> GetGuestRequestsByCondition(Predicate<GuestRequest> predicate);
        int GetAmountOrders(GuestRequest guestRequest);
        int GetAmountOrders(HostingUnit hostingUnit);

        IEnumerable<IGrouping<bool, GuestRequest>> GetGuestRequestsGroupByArea(Regions area);
        IEnumerable<IGrouping<int, GuestRequest>> GetGuestRequestsGroupByVacationersNumber();
        IEnumerable<IGrouping<int, Host>> GetHostsGroupByNumOfUnits();
        IEnumerable<IGrouping<BankBranch, Host>> GetHostsGroupByBankBranch();
        IEnumerable<IGrouping<bool, HostingUnit>> GetHostingUnitsGroupByArea(Regions area);

        HostingUnit GetHostingUnit(int key);
        Order GetOrder(int key);
        Host GetHost(int key);
    }
}
