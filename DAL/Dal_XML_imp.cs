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
            DSXML.SaveToXMLSerialize(hostingUnit, "HostingUnit");
            return hostingUnit.HostingUnitKey;
        }

        public int AddOrder(Order order)
        {
            order.OrderKey = orderSerialKey++;

            DS.DSXML.Orders.Add(order.ToXML());
            DS.DSXML.SaveOrders();

            DS.DSXML.Configs.Element("orderSerialKey").Value = orderSerialKey.ToString();
            DS.DSXML.SaveConfigs();

            return order.OrderKey;
        }

        public List<BankBranch> GetBankBranches()
        {
            
        }

        public List<GuestRequest> GetGuestRequests()
        {
            return DSXML.LoadFromXMLSerialize<List<GuestRequest>>("GuestRequest");
        }

        public List<HostingUnit> GetHostingUnits()
        {
            return DSXML.LoadFromXMLSerialize<List<HostingUnit>>("HostingUnit");
        }

        public List<Host> GetHosts()
        {
            return DSXML.LoadFromXMLSerialize<List<Host>>("Host");
        }

        public List<Order> GetOrders()
        {
            return DSXML.LoadFromXMLSerialize<List<Order>>("Order");
        }

        public void RemoveHostingUnit(int key)
        {
            var hostingUnit = DSXML.HostingUnits.Elements("HostingUnit")
                .FirstOrDefault(k => key == int.Parse(k.Element("HostingUnitKey").Value));

            if (null == hostingUnit)
                throw new KeyNotFoundException("יחידת אירוח לא קיימת");
            else
                hostingUnit.Remove();
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            var oldGR = (from GR in DSXML.GuestRequests.Elements("GuestRequest")
                    where int.Parse(GR.Element("guestRequestKey").Value) == guestRequest.guestRequestKey
                    select GR)
                    .FirstOrDefault();

            if (oldGR != null)
            {
                oldGR.Remove();
                DSXML.GuestRequests.Add(guestRequest.ToXML());
            }
            else
                throw new ArgumentException("דרישת לקוח לא קיימת") { Source = "DAL" };
        }

        public void UpdateHost(Host host)
        {
            var oldH = (from h in DSXML.Hosts.Elements("Host")
                    where int.Parse(h.Element("HostKey").Value) == host.HostKey
                    select h).FirstOrDefault();
            if (oldH != null)
            {
                oldH.Remove();
                DSXML.Hosts.Add(host.ToXML());
            }
            else
                throw new ArgumentException("המארח לא קיים") { Source = "DAL" };
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            var old = DSXML.HostingUnits.Elements("hostingUnit")
                .FirstOrDefault(hu => int.Parse(hu.Element("HostingUnitKey").Value) == hostingUnit.HostingUnitKey);

            if (old != null)
            {
                old.Remove();
                DSXML.SaveToXMLSerialize(hostingUnit, "HostingUnit");
            }
            else
                throw new ArgumentException("יחידת אירוח לא קיימת") { Source = "DAL" };
        }

        public void UpdateOrder(Order order)
        {
            var old = (from O in DSXML.Orders.Elements("Order")
                    where int.Parse(O.Element("OrderKey").Value) == order.OrderKey
                    select O).FirstOrDefault();
            if (old != null)
            {
                old.Remove();
                DSXML.Orders.Add(order.ToXML());
            }
            else
                throw new ArgumentException("הזמנה לא קיימת") { Source = "DAL" };
        }
    }
}
