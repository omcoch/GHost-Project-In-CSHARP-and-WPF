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


        public void AddGuestRequest(GuestRequest guestRequest)
        {
            DS.DataSource.GuestRequests.Add(guestRequest);
        }



        public void AddHostingUnit(HostingUnit hostingUnit)
        {
            DS.DataSource.HostingUnits.Add(hostingUnit);
        }


        public void AddOrder(Order order)
        {
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

        public void RemoveHostingUnit()
        {
            throw new NotImplementedException();
        }

        public void UpdateGuestRequest()
        {
            throw new NotImplementedException();
        }

        public void UpdateHostingUnit()
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder()
        {
            throw new NotImplementedException();
        }
    }
}
