using BE;
using DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class Dal_imp : IDAL
    {

        protected Dal_imp() { }
        protected static Dal_imp instance = null;
        public static Dal_imp GetInstance()
        {
            if (instance == null)
                instance = new Dal_imp();
            return instance;
        }

        public void AddGuestRequest(GuestRequest guestRequest)
        {
            guestRequest.guestRequestKey = Configuration.GuestRequestSerialKey;
            DS.DataSource.GuestRequests.Add(guestRequest);
        }


        public void AddHostingUnit(HostingUnit hostingUnit)
        {
            var v = from HU in DS.DataSource.HostingUnits
                    where HU.Owner.HostKey == hostingUnit.Owner.HostKey
                    select HU.Owner.HostKey;
            if (!v.Any())
                DS.DataSource.HostingUnits.Add(hostingUnit); 
        }


        public void AddOrder(Order order)
        {
            order.OrderKey = Configuration.OrderSerialKey;
            DS.DataSource.Orders.Add(order);
        }

        public List<BankBranch> GetBankBranches()
        {
            List<BankBranch> bankBranches = new List<BankBranch>() {
               new BankBranch (){ BankNumber=12,BankName="בנק הפועלים",BranchNumber=723,BranchAddress="הגליל",BranchCity="טבריה" },
               new BankBranch (){ BankNumber=11,BankName="בנק דיסקונט",BranchNumber=41,BranchAddress="יפו 220",BranchCity="ירושלים" },
               new BankBranch (){ BankNumber=10,BankName="בנק לאומי",BranchNumber=152,BranchAddress="דיזינגוף",BranchCity="תל אביב" },
               new BankBranch (){ BankNumber=13,BankName="בנק אוצר החייל",BranchNumber=723,BranchAddress="תל חי 56",BranchCity="כפר סבא" },
               new BankBranch (){ BankNumber=12,BankName="בנק חדש",BranchNumber=723,BranchAddress="הרב עובדיה",BranchCity="ירושלים" }
            };
            return bankBranches;
        }

        public List<GuestRequest> GetGuestRequests()
        {
            List<GuestRequest> guestRequests = DS.DataSource.GuestRequests.ToList();
            return guestRequests;
        }

        public List<HostingUnit> GetHostingUnits()
        {
            List<HostingUnit> hostingUnits = DS.DataSource.HostingUnits.ToList();
            return hostingUnits;
        }

        public List<Order> GetOrders()
        {
            List<Order> orders = DS.DataSource.Orders.ToList();
            return orders;
        }

        public void RemoveHostingUnit(int key)
        {
            var v = (from HU in DataSource.HostingUnits
                    where HU.HostingUnitKey == key
                    select HU).First();
            DataSource.HostingUnits.Remove(v);
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            var v = from GR in DataSource.GuestRequests
                    where GR.guestRequestKey == guestRequest.guestRequestKey
                    select GR;
            DataSource.GuestRequests.Remove(v.First());
            DataSource.GuestRequests.Add(guestRequest);
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            var v = from HU in DataSource.HostingUnits
                    where HU.HostingUnitKey == hostingUnit.HostingUnitKey
                    select HU;
            DataSource.HostingUnits.Remove(v.First());
            DataSource.HostingUnits.Add(hostingUnit);
        }

        public void UpdateOrder(Order order)
        {
            var v = from O in DataSource.Orders
                    where O.OrderKey == order.OrderKey
                    select O;
            DataSource.Orders.Remove(v.First());
            DataSource.Orders.Add(order);
        }
    }
}
