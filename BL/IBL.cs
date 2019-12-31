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
        void AddGuestRequest(GuestRequest guestRequest);
        void UpdateGuestRequest(GuestRequest guestRequest);

        void AddHostingUnit(HostingUnit hostingUnit);
        void RemoveHostingUnit(int key);
        void UpdateHostingUnit(HostingUnit hostingUnit);

        void AddOrder(Order order);
        void UpdateOrder(Order order);

    }
}
