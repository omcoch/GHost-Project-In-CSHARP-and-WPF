using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    class Dal_XML_imp : IDAL
    {
        private static int guestRequestSerialKey;//מספר מזהה רץ עבור בקשת לקוח
        private static int orderSerialKey;//מספר מזהה רץ עבור הזמנה
        private static int hostingUnitSerialKey;//מספר מזהה רץ עבור יחדית אירוח

        protected Dal_XML_imp()
        {
            guestRequestSerialKey = int.Parse(DS.DSXML.Configs.Element("guestRequestSerialKey").Value);
            orderSerialKey = int.Parse(DS.DSXML.Configs.Element("orderSerialKey").Value);
            hostingUnitSerialKey = int.Parse(DS.DSXML.Configs.Element("hostingUnitSerialKey").Value);
        }

        protected static Dal_XML_imp instance = null;
        public static Dal_XML_imp GetInstance()
        {
            if (instance == null)
                instance = new Dal_XML_imp();
            return instance;
        }
        //todo: לבדוק שיש שורש לקובץ XML ושADD מוסיף תחתיו
        public int AddGuestRequest(GuestRequest guestRequest)
        {
            guestRequest.guestRequestKey = guestRequestSerialKey++;

            DS.DSXML.GuestRequests.Add(guestRequest.ToXML());
            DS.DSXML.SaveGuestRequests();

            DS.DSXML.Configs.Element("guestRequestSerialKey").Value = guestRequestSerialKey.ToString();
            DS.DSXML.SaveConfigs();

            return guestRequest.guestRequestKey;
        }

        public int AddHost(Host host)
        {
            XElement findHost = (from h in DSXML.Hosts.Elements("Host")
                                  where int.Parse(h.Element("HostKey").Value) == host.HostKey
                                 select h).FirstOrDefault();
            if (findHost != null)
                throw new ArgumentException("המארח קיים כבר");

            DS.DSXML.Hosts.Add(host.ToXML());
            DS.DSXML.SaveHosts();

            return host.HostKey;
        }

        public int AddHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnit.HostingUnitKey = hostingUnitSerialKey++;

            
            hostingUnit.Diary = new bool[12, 31]; // יצירת יומן חדש
            hostingUnit.
            return hostingUnit.HostingUnitKey;
        }

        public int AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public List<BankBranch> GetBankBranches()
        {
            throw new NotImplementedException();
        }

        public List<GuestRequest> GetGuestRequests()
        {
            throw new NotImplementedException();
        }

        public List<HostingUnit> GetHostingUnits()
        {
            throw new NotImplementedException();
        }

        public List<Host> GetHosts()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders()
        {
            throw new NotImplementedException();
        }

        public void RemoveHostingUnit(int key)
        {
            throw new NotImplementedException();
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            throw new NotImplementedException();
        }

        public void UpdateHost(Host host)
        {
            throw new NotImplementedException();
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
