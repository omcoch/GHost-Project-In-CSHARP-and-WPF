using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDAL
    {
        void AddGuestRequest(GuestRequest guestRequest);
        void UpdateGuestRequest();

        void AddHostingUnit(HostingUnit hostingUnit);
        void RemoveHostingUnit();
        void UpdateHostingUnit();

        void AddOrder(Order order);
        void UpdateOrder();

        List<GuestRequest> GetGuestRequests();
        List<HostingUnit> GetHostingUnits();
        List<Order> GetOrders();

        List<BankBranch> GetBankBranches();
    }

}
