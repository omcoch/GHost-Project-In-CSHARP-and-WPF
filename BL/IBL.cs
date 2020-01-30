using BE;
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

        string AddHost(Host host);
        void UpdateHost(Host host);

        int AddHostingUnit(HostingUnit hostingUnit);
        void RemoveHostingUnit(int key);
        void UpdateHostingUnit(HostingUnit hostingUnit);

        int AddOrder(Order order);
        void UpdateOrder(Order order, ref System.Net.Mail.MailMessage message);

        List<HostingUnit> GetAvailableHostingUnits(DateTime date, int days);
        int TimeDistance(DateTime first, DateTime last = default(DateTime));

        List<Order> GetOrdersBefore(int days);
        /// <summary>
        /// מחזירה רשימה של דרישות לקוח לפי תנאי מסויים
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<GuestRequest> GetGuestRequestsByCondition(Predicate<GuestRequest> predicate);
        int GetAmountOrders(GuestRequest guestRequest);
        int GetAmountOrders(HostingUnit hostingUnit);

        IEnumerable<IGrouping<bool, GuestRequest>> GetGuestRequestsGroupByArea(Regions area);
        IEnumerable<IGrouping<Regions, GuestRequest>> GetGuestRequestsGroupByArea();
        IEnumerable<IGrouping<int, GuestRequest>> GetGuestRequestsGroupByVacationersNumber();
        IEnumerable<IGrouping<int, Host>> GetHostsGroupByNumOfUnits();
        IEnumerable<IGrouping<BankBranch, Host>> GetHostsGroupByBankBranch();
        IEnumerable<IGrouping<bool, HostingUnit>> GetHostingUnitsGroupByArea(Regions area);
        IEnumerable<IGrouping<Regions, HostingUnit>> GetHostingUnitsGroupByArea();

        HostingUnit GetHostingUnit(int key);
        List<HostingUnit> GetHostingUnitsByOwner(string key);
        List<Order> GetOrdersByCondition(Predicate<Order> predicate);
        List<Order> GetOrdersByHostKey(string key);
        Order GetOrder(int key);
        Host GetHost(string key);
    }
}
