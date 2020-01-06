using BE;
using DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public int AddGuestRequest(GuestRequest gR)
        {
            GuestRequest guestRequest = Cloning.Clone(gR);
            guestRequest.guestRequestKey = Configuration.GenerateGuestRequestSerialKey;
            DataSource.GuestRequests.Add(guestRequest);
            return guestRequest.guestRequestKey;
        }


        public int AddHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnit = Cloning.Clone(hostingUnit);
            var v = from HU in DataSource.HostingUnits
                    where HU.Owner.HostKey == hostingUnit.Owner.HostKey
                    select HU.Owner.HostKey;
            if (!v.Any())
            {
                hostingUnit.HostingUnitKey = Configuration.GenerateHostingUnitSerialKey;
                hostingUnit.Diary = new bool[12, 31];
                DataSource.HostingUnits.Add(hostingUnit);
                return hostingUnit.HostingUnitKey;
            }
            else
                throw new ArgumentException("יחידת אירוח כבר קיימת במאגר");
        }


        public int AddOrder(Order order)
        {
            order = Cloning.Clone(order);
            order.OrderKey = Configuration.GenerateOrderSerialKey;
            DataSource.Orders.Add(order);
            return order.OrderKey;
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
            List<GuestRequest> guestRequests = DataSource.GuestRequests.Select(item=>item).ToList();
            return guestRequests;
        }

        public List<HostingUnit> GetHostingUnits()
        {
            List<HostingUnit> hostingUnits = DS.DataSource.HostingUnits.Select(item => item).ToList();
            return hostingUnits;
        }

        public List<Order> GetOrders()
        {
            List<Order> orders = DS.DataSource.Orders.Select(item => item).ToList();
            return orders;
        }

        public void RemoveHostingUnit(int key)
        {

            var hostingUnit = DataSource.HostingUnits.FirstOrDefault(k => key == k.HostingUnitKey);
            if (null == hostingUnit)
                throw new KeyNotFoundException("יחידת אירוח לא קיימת");
            else
                DataSource.HostingUnits.Remove(hostingUnit);
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            guestRequest = Cloning.Clone(guestRequest);
            var v = from GR in DataSource.GuestRequests
                    where GR.guestRequestKey == guestRequest.guestRequestKey
                    select GR;
            if (v.Any())
            {
                DataSource.GuestRequests.Remove(v.First());
                DataSource.GuestRequests.Add(guestRequest);
            }
            else
                throw new ArgumentException("דרישת לקוח לא קיימת");
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnit = Cloning.Clone(hostingUnit);
            var v = DataSource.HostingUnits.FirstOrDefault(hu => hu.HostingUnitKey == hostingUnit.HostingUnitKey);
            if (DataSource.HostingUnits.Remove(v))
                DataSource.HostingUnits.Add(hostingUnit);
            else
                throw new ArgumentException("יחידת אירוח לא קיימת");
        }

        public void UpdateOrder(Order order)
        {
            order = Cloning.Clone(order);
            var v = from O in DataSource.Orders
                    where O.OrderKey == order.OrderKey
                    select O;
            if (v.Any())
            {
                DataSource.Orders.Remove(v.First());
                DataSource.Orders.Add(order);
            }
            else
                throw new ArgumentException("הזמנה לא קיימת");
        }

        
    }
}
