using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
using System.Threading;

namespace DAL
{
    class Dal_XML_imp : IDAL
    {
        private static int guestRequestSerialKey;//מספר מזהה רץ עבור בקשת לקוח
        private static int orderSerialKey;//מספר מזהה רץ עבור הזמנה
        private static int hostingUnitSerialKey;//מספר מזהה רץ עבור יחדית אירוח

        public static List<HostingUnit> HostingUnits;

        protected Dal_XML_imp()
        {
            Thread bankAccunDownload = new Thread(DSXML.DownloadBankXml);
            bankAccunDownload.Start();

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
            XElement findHost = (from h in DSXML.Hosts.Elements()
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
            HostingUnits = DSXML.LoadFromXMLSerialize<List<HostingUnit>>(DSXML.hostingUnitPath);
            hostingUnit.HostingUnitKey = hostingUnitSerialKey++;            
            hostingUnit.Diary = new bool[12, 31]; // יצירת יומן חדש
            HostingUnits.Add(hostingUnit);
            DSXML.SaveToXMLSerialize(HostingUnits, DSXML.hostingUnitPath);
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
            return (from item in DSXML.BankBranches.Elements()
                    select new BankBranch()
                    {
                        BankName = item.Element("שם_בנק").Value,
                        BankNumber = int.Parse(item.Element("קוד_בנק").Value),
                        BranchAddress = item.Element("כתובת_ה-ATM").Value,
                        BranchCity = item.Element("ישוב").Value,
                        BranchNumber = int.Parse(item.Element("קוד_סניף").Value),
                    }
                ).ToList();
        }

        public List<GuestRequest> GetGuestRequests()
        {

            return (from gr in DS.DSXML.GuestRequests.Elements()
                    select new GuestRequest()
                    {
                        Adults = int.Parse(gr.Element("Adults").Value),
                        Children = int.Parse(gr.Element("Children").Value),
                        Area = (Regions)Enum.Parse(typeof(Regions),gr.Element("Area").Value),
                        EntryDate = DateTime.Parse(gr.Element("EntryDate").Value),
                        FamilyName = gr.Element("FamilyName").Value,
                        ChildrensAttractions = (Requirements)Enum.Parse(typeof(Requirements), gr.Element("ChildrensAttractions").Value),
                        Garden = (Requirements)Enum.Parse(typeof(Requirements), gr.Element("Garden").Value),
                        Jacuzzi = (Requirements)Enum.Parse(typeof(Requirements), gr.Element("Jacuzzi").Value),
                        Pool = (Requirements)Enum.Parse(typeof(Requirements), gr.Element("Pool").Value),
                        MailAddress = new System.Net.Mail.MailAddress(gr.Element("MailAddress").Value),
                        MaxPrice = double.Parse(gr.Element("MaxPrice").Value),
                        PrivateName = gr.Element("PrivateName").Value,
                        guestRequestKey = int.Parse(gr.Element("guestRequestKey").Value),
                        RegistrationDate = DateTime.Parse(gr.Element("RegistrationDate").Value),
                        ReleaseDate = DateTime.Parse(gr.Element("ReleaseDate").Value),
                        Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), gr.Element("Status").Value),
                        SubArea = gr.Element("SubArea").Value,
                        Type = (GRType)Enum.Parse(typeof(GRType), gr.Element("Type").Value),
                    }
                    ).ToList();
        }

        public List<HostingUnit> GetHostingUnits()
        {
            return DSXML.LoadFromXMLSerialize<List<HostingUnit>>(DSXML.hostingUnitPath);
        }

        public List<Host> GetHosts()
        {
            return (from h in DS.DSXML.Hosts.Elements()
                    select new Host()
                    {
                        HostKey = int.Parse(h.Element("HostKey").Value),
                        ChargeAmount = int.Parse(h.Element("ChargeAmount").Value),
                        MailAddress = new System.Net.Mail.MailAddress(h.Element("MailAddress").Value),
                        PhoneNumber = h.Element("PhoneNumber").Value,
                        FamilyName = h.Element("FamilyName").Value,
                        NumOfHostingUnits= int.Parse(h.Element("NumOfHostingUnits").Value),
                        PrivateName = h.Element("PrivateName").Value,
                        CollectionClearance = bool.Parse(h.Element("CollectionClearance").Value),
                        BankAccountNumber = int.Parse(h.Element("BankAccountNumber").Value),
                        BankAccountDetails = new BankBranch()
                        {
                            BankName = h.Element("BankAccountDetails").Element("BankName").Value,
                            BankNumber = int.Parse(h.Element("BankAccountDetails").Element("BankNumber").Value),
                            BranchAddress = h.Element("BankAccountDetails").Element("BranchAddress").Value,
                            BranchCity = h.Element("BankAccountDetails").Element("BranchCity").Value,
                            BranchNumber = int.Parse(h.Element("BankAccountDetails").Element("BranchNumber").Value),
                        }
                    }).ToList();
        }

        public List<Order> GetOrders()
        {
            return (from o in DS.DSXML.Orders.Elements()
                    select new Order()
                    {
                        CreateDate = DateTime.Parse(o.Element("CreateDate").Value),
                        OrderDate = DateTime.Parse(o.Element("OrderDate").Value),
                        GuestRequestKey = int.Parse(o.Element("GuestRequestKey").Value),
                        HostingUnitKey = int.Parse(o.Element("HostingUnitKey").Value),
                        OrderKey = int.Parse(o.Element("OrderKey").Value),
                        Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), o.Element("Status").Value)                        
                    }
                    ).ToList();
        }

        public void RemoveHostingUnit(int key)
        {
            var list = DSXML.LoadFromXMLSerialize<List<HostingUnit>>(DSXML.hostingUnitPath);
            var newList = list.Where(hu => hu.HostingUnitKey != key).ToList();

            if (list.Count != newList.Count)
                DSXML.SaveToXMLSerialize(newList, DSXML.hostingUnitPath);
            else
                throw new KeyNotFoundException("יחידת אירוח לא קיימת");
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            var g = (from GR in DSXML.GuestRequests.Elements()
                    where int.Parse(GR.Element("guestRequestKey").Value) == guestRequest.guestRequestKey
                    select GR)
                    .FirstOrDefault();

            if (g != null)
            {
                g.Element("Status").Value = guestRequest.Status.ToString(); // עדכון מתבצע רק לסטטוס, בעת סגירת הזמנות
                DSXML.SaveGuestRequests();
            }
            else
                throw new ArgumentException("דרישת לקוח לא קיימת") { Source = "DAL" };
        }

        public void UpdateHost(Host host)
        {
                XElement tempElement = (from h in DSXML.Hosts.Elements()
                                        where int.Parse(h.Element("HostKey").Value) == host.HostKey
                                        select h).FirstOrDefault();
                if (tempElement == null)
                    throw new ArgumentException("המארח לא קיים") { Source = "DAL" };

                tempElement.Element("HostKey").Value = host.HostKey.ToString();
                tempElement.Element("PrivateName").Value = host.PrivateName;
                tempElement.Element("FamilyName").Value = host.FamilyName;
                tempElement.Element("PhoneNumber").Value = host.PhoneNumber;
                tempElement.Element("MailAddress").Value = host.mailAddress;
                tempElement.Element("BankAccountDetails").Element("BankNumber").Value = host.BankAccountDetails.BankNumber.ToString();
                tempElement.Element("BankAccountDetails").Element("BankName").Value = host.BankAccountDetails.BankName;
                tempElement.Element("BankAccountDetails").Element("BranchNumber").Value = host.BankAccountDetails.BranchNumber.ToString();
                tempElement.Element("BankAccountDetails").Element("BranchAddress").Value = host.BankAccountDetails.BranchAddress;
                tempElement.Element("BankAccountDetails").Element("BranchCity").Value = host.BankAccountDetails.BranchCity;
                tempElement.Element("BankAccountNumber").Value = host.BankAccountNumber.ToString();
                tempElement.Element("NumOfHostingUnits").Value = host.NumOfHostingUnits.ToString();
                tempElement.Element("ChargeAmount").Value = host.ChargeAmount.ToString();
                tempElement.Element("CollectionClearance").Value = host.CollectionClearance?"true":"false";

                DSXML.SaveHosts();            
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            var list = DSXML.LoadFromXMLSerialize<List<HostingUnit>>(DSXML.hostingUnitPath);
            var newList = list.Where(hu => hu.HostingUnitKey != hostingUnit.HostingUnitKey).ToList();
            newList.Add(hostingUnit);
            if (list.Count != newList.Count)
                throw new ArgumentException("יחידת אירוח לא קיימת") { Source = "DAL" };
            else
            {
                DSXML.SaveToXMLSerialize(newList, DSXML.hostingUnitPath);
            }         
                
        }

        public void UpdateOrder(Order order)
        {
            var old = (from O in DSXML.Orders.Elements()
                    where int.Parse(O.Element("OrderKey").Value) == order.OrderKey
                    select O).FirstOrDefault();
            if (old != null)
            {
                old.Element("Status").Value = order.Status.ToString();
                old.Element("OrderDate").Value = order.OrderDate.ToString();
                DSXML.SaveOrders();
            }
            else
                throw new ArgumentException("הזמנה לא קיימת") { Source = "DAL" };
        }
    }
}
