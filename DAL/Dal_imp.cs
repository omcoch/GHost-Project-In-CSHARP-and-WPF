using BE;
using DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    class Dal_xml_imp : IDAL
    {
        
        protected Dal_xml_imp() { }
        protected static Dal_xml_imp instance = null;
        public static Dal_xml_imp GetInstance()
        {
            if (instance == null)
                instance = new Dal_xml_imp();
            return instance;
        }

        public int AddGuestRequest(GuestRequest gR)
        {
            // לא מבצעים בדיקה האם מספר הזיהוי קיים כבר כי ה DAL
            // בעצמו יוצר את מספר הזיהוי ולא יכול להיות שנכניס 2 אובייקטים עם מספר זיהוי זהה
            GuestRequest guestRequest = Cloning.Clone(gR);
            guestRequest.guestRequestKey = Configuration.GenerateGuestRequestSerialKey;
            DataSource.GuestRequests.Add(guestRequest);
            return guestRequest.guestRequestKey;
        }


        public int AddHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnit.HostingUnitKey = Configuration.GenerateHostingUnitSerialKey;
            hostingUnit.Diary = new bool[12, 31];
            DataSource.HostingUnits.Add(hostingUnit);
            return hostingUnit.HostingUnitKey;
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
               new BankBranch (){ BankNumber=14,BankName="בנק חדש",BranchNumber=723,BranchAddress="הרב עובדיה",BranchCity="ירושלים" }
            };
            return bankBranches;
        }

        public List<GuestRequest> GetGuestRequests()
        {
            List<GuestRequest> guestRequests = DataSource.GuestRequests.Select(item=> Cloning.Clone(item)).ToList();
            return guestRequests;
        }

        public List<HostingUnit> GetHostingUnits()
        {
            List<HostingUnit> hostingUnits = DS.DataSource.HostingUnits.Select(item => Cloning.Clone(item)).ToList();
            return hostingUnits;
        }

        public List<Order> GetOrders()
        {
            List<Order> orders = DS.DataSource.Orders.Select(item => Cloning.Clone(item)).ToList();
            return orders;
        }

        public void RemoveHostingUnit(int key)
        {

            var hostingUnit = DataSource.HostingUnits.FirstOrDefault(k => key == k.HostingUnitKey);
            if (null == hostingUnit)
                throw new KeyNotFoundException("יחידת אירוח לא קיימת") { Source = "DAL" };
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
                throw new ArgumentException("דרישת לקוח לא קיימת") { Source = "DAL" };
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnit = Cloning.Clone(hostingUnit);
            var v = DataSource.HostingUnits.FirstOrDefault(hu => hu.HostingUnitKey == hostingUnit.HostingUnitKey);
            if (DataSource.HostingUnits.Remove(v))
                DataSource.HostingUnits.Add(hostingUnit);
            else
                throw new ArgumentException("יחידת אירוח לא קיימת") { Source = "DAL" };
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
                throw new ArgumentException("הזמנה לא קיימת") { Source = "DAL" };
        }

        
    }
}
