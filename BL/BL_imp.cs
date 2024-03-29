﻿using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Utilities;

namespace BL
{
    public class BL_imp : IBL
    {
        protected static BL_imp instance = null;
        public static BL_imp GetInstance()
        {
            if (instance == null)
                instance = new BL_imp();
            return instance;
        }


        IDAL dal;
        public BL_imp()
        {
            dal = DalFactory.getDal();

            Thread UpdateStatus = new Thread(CloseOrdersBefore);
            UpdateStatus.Start();
        }

        /// <summary>
        /// סוגר הזמנות ישנות שנפתחו לפני יותר מ30 ימים.
        /// </summary>
        private void CloseOrdersBefore()
        {
            var orders = GetOrdersBefore(30);
            foreach(var item in orders)
            {
                item.Status = OrderStatus.נסגר_מחוסר_הענות_של_הלקוח;
                dal.UpdateOrder(item);
            }
        }

        public int AddGuestRequest(GuestRequest guestRequest)
        {
            if (TimeDistance(guestRequest.EntryDate, guestRequest.ReleaseDate) < 1)
                throw new ArgumentOutOfRangeException("על תאריך תחילת הנופש להיות קודם לפחות ביום אחד לתאריך סיום הנופש");
            return dal.AddGuestRequest(guestRequest);
        }


        public int AddHostingUnit(HostingUnit hostingUnit)
        {
            Host owner = dal.GetHosts().FirstOrDefault(h => hostingUnit.OwnerKey == h.HostKey);
            try
            {
                owner.NumOfHostingUnits++;//מגדילים את מספר היחידות שבבעלות המארח באחד
                dal.UpdateHost(owner);
                return dal.AddHostingUnit(hostingUnit);
            }
            catch (ArgumentException e)
            {
                owner.NumOfHostingUnits--;//במקרה שלא הצליח להוסיף את יחידת האירוח נוריד את מספר יחידות האירוח שבבעלות המארח
                dal.UpdateHost(owner);
                throw e;
            }
        }

        public int AddOrder(Order order)
        {
            //יומן של יחידת אירוח
            var diaryOfHostingUnit = (from hu in dal.GetHostingUnits()
                                      where hu.HostingUnitKey == order.HostingUnitKey
                                      select hu.Diary).FirstOrDefault();
            //תאריכי נופש של דרישת לקוח
            var guestRequestDates = dal.GetGuestRequests()
                .Where(x => x.guestRequestKey == order.GuestRequestKey)
                .Select(x => new { entryDate = x.EntryDate, releaseDate = x.ReleaseDate }).FirstOrDefault();

            if (null != guestRequestDates && null != diaryOfHostingUnit)
            {
                DateTime entryDate = guestRequestDates.entryDate,
                    releaseDate = guestRequestDates.releaseDate;

                if (!CheckDiary(entryDate, releaseDate, diaryOfHostingUnit))
                    throw new DateOccupiedException("התאריך תפוס");
                return dal.AddOrder(order);
            }
            else
                throw new ExecutionOrderException("דרישת לקוח/יחידת אירוח לא קיימת");
        }

        private bool CheckDiary(DateTime entryDate, DateTime releaseDate, bool[,] diary)
        {
            while (entryDate <= releaseDate)
            {
                if (diary[entryDate.Month-1, entryDate.Day-1])
                    return false;
                entryDate = entryDate.AddDays(1);
            }
            return true;
        }

        public void RemoveHostingUnit(int key)
        {
            var v = from order in dal.GetOrders()
                    where order.HostingUnitKey == key
                    && (order.Status == OrderStatus.טרם_טופל || order.Status == OrderStatus.נשלח_מייל)
                    select order;
            if (!v.Any())
                dal.RemoveHostingUnit(key);
            else
                throw new ArgumentException("ליחידת אירוח זו קיימות הזמנות פתוחות");
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            TimeSpan timeSpan = guestRequest.ReleaseDate - guestRequest.EntryDate;
            if (timeSpan.TotalDays < 1)
                throw new ArgumentOutOfRangeException("על תאריך תחילת הנופש להיות קודם לפחות ביום אחד לתאריך סיום הנופש");

            var v = from gr in dal.GetGuestRequests()
                    where gr.guestRequestKey == guestRequest.guestRequestKey
                    select gr;
            if (!v.Any())
                throw new ArgumentException("דרישת לקוח אינה קיימת במקור הנתונים");
            dal.UpdateGuestRequest(guestRequest);
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            dal.UpdateHostingUnit(hostingUnit);
        }

        public void UpdateOrder(Order Order,ref MailMessage mailMessage)
        {
            // יחידת האירוח הקשורה להזמנה
            var hostingUnit = (from h in dal.GetHostingUnits()
                               where h.HostingUnitKey == Order.HostingUnitKey
                               select h).FirstOrDefault();
            // סטטוס ההזמנה כפי ששמור במקור הנתונים
            var os = from order in dal.GetOrders()
                     where order.OrderKey == Order.OrderKey
                     select order.Status;
            // דרישת לקוח הקשורה להזמנה
            var guestRequest = (from GR in dal.GetGuestRequests()
                                where GR.guestRequestKey == Order.GuestRequestKey
                                select GR).FirstOrDefault();
            // מארח של ההזמנה
            Host owner = dal.GetHosts().FirstOrDefault(h => hostingUnit.OwnerKey == h.HostKey);

            if (!os.Any())
                throw new ArgumentException("ההזמנה לא קיימת במקור הנתונים");

            OrderStatus orderStatus = os.First();
            if (orderStatus == OrderStatus.נסגר_בהיענות_של_הלקוח || orderStatus == OrderStatus.נסגר_מחוסר_הענות_של_הלקוח
                ||orderStatus==OrderStatus.נסגר_בעקבות_סגירת_עסקה_עם_מארח_אחר)
                throw new ExecutionOrderException("הבקשה כבר סגורה");

            if (owner.CollectionClearance)
                try
                {
                    if (!CheckDiary(guestRequest.EntryDate, guestRequest.ReleaseDate, hostingUnit.Diary))
                        throw new DateOccupiedException("תאריך הנופש תפוס");
                    if (Order.Status == OrderStatus.נשלח_מייל)
                    {
                        mailMessage=SendMailToGuest(Order.GuestRequestKey, hostingUnit, guestRequest); 
                        Order.OrderDate = DateTime.Now;
                    }
                    // עדכון ההזמנה
                    dal.UpdateOrder(Order);

                    // עדכון הזמנה כאשר האירוח התקיים
                    if (Order.Status == OrderStatus.נסגר_בהיענות_של_הלקוח)
                    {

                        // במידה ונסגרה ההעסקה בינהם נבצע חישוב עמלה
                        owner.ChargeAmount += CalculateFee((guestRequest.ReleaseDate - guestRequest.EntryDate).Days);

                        // מילוי המטריצה בתאריכים המבוקשים
                        FillDiary(hostingUnit, guestRequest.EntryDate, guestRequest.ReleaseDate);

                        // עדכון יחידת האירוח בבסיס הנתונים
                        dal.UpdateHostingUnit(hostingUnit);

                        // עדכון סטטוס דרישת לקוח
                        guestRequest.Status = RequestStatus.נסגרה_דרך_האתר;
                        dal.UpdateGuestRequest(guestRequest);

                        // עדכון העמלה אצל המארח
                        dal.UpdateHost(owner);

                        // עדכון שאר ההזמנות של אותה דרישת לקוח כסגורות
                        var orders = from o in dal.GetOrders()
                                     where o.GuestRequestKey == guestRequest.guestRequestKey && o.OrderKey != Order.OrderKey
                                     select o;
                        foreach (var order in orders)
                        {
                            order.Status = OrderStatus.נסגר_בעקבות_סגירת_עסקה_עם_מארח_אחר;
                            dal.UpdateOrder(order);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            else
                throw new ExecutionOrderException("לא בוצע אישור לחיוב חשבון");
        }

        /// <summary>
        /// יצירת הודעת מייל ללקוח בגין הזמנה שנוצרה עבורו
        /// </summary>
        /// <param name="orderKey">מספר הזמנה</param>
        /// <param name="guestRequest">דרישת לקוח</param>
        /// <returns>מחזיר את הודעת המייל</returns>
        private MailMessage SendMailToGuest(int orderKey, HostingUnit hostingUnit, GuestRequest guestRequest)
        {
            MailAddress mailAddress = dal.GetHosts().Where(h => hostingUnit.OwnerKey == h.HostKey).Select(o => o.MailAddress).First();
            MailMessage message = new MailMessage();
            message.From = mailAddress;
            message.To.Add(guestRequest.MailAddress.Address);
            message.Subject = "נוצרה הזמנה עבור דרישת לקוח מספר " + guestRequest.guestRequestKey;
            message.Body = "שלום, " + guestRequest.PrivateName + "\nנפתחה עבורך הזמנה לאירוח אצל " + hostingUnit.HostingUnitName
                + ".\nמספר ההזמנה: " + orderKey + "\nאנא צור קשר עם המארח בכתובת " + mailAddress + "\nבברכת חופשה מהנה, \n" + Configuration.SiteName;
            return message;
        }
        /// <summary>
        /// מחשב את העמלה
        /// </summary>
        /// <param name="amountDays">מספר ימי הנופש</param>
        /// <returns>סכום העמלה</returns>
        int CalculateFee(int amountDays)
        {
            return amountDays * Configuration.FEE;
        }
        /// <summary>
        /// ממלא את היומן
        /// </summary>
        /// <param name="hostingUnit">יחידת האירוח שעבורה אנו רוצים למלא את היומן</param>
        /// <param name="entry">תאריך תחילת הנופש</param>
        /// <param name="release">תאריך סיום הנופש</param>
        void FillDiary(HostingUnit hostingUnit, DateTime entry, DateTime release)
        {
            while (entry <= release)
            {
                hostingUnit.Diary[entry.Month-1, entry.Day-1] = true;
                entry = entry.AddDays(1);
            }
        }
        /// <summary>
        /// פונקציה המחזירה את יחידות האירוח הפנויות בתאריך מסויים
        /// </summary>
        /// <param name="date">תאריך תחילת הנופש</param>
        /// <param name="days">מספר ימי הנופש</param>
        /// <returns>רשימת יחידות האירוח הפנויות בתאריך הנתון</returns>
        public List<HostingUnit> GetAvailableHostingUnits(DateTime date, int days)
        {
            var v = (from unit in dal.GetHostingUnits()
                     where CheckDiary(date, date.AddDays(days), unit.Diary) == true
                     select unit).ToList();
            return v;
        }

        public int TimeDistance(DateTime first, DateTime last = default(DateTime))
        {
            if (last == default(DateTime))
                last = DateTime.Now;
            return (last - first).Days;
        }

        public List<Order> GetOrdersBefore(int days)
        {
            // (using delegate anonymous function)
            return dal.GetOrders()
                .FindAll(delegate (Order order)
                {
                    return TimeDistance(order.CreateDate) >= days || TimeDistance(order.OrderDate) >= days;
                });
        }
        /// <summary>
        /// מחזירה רשימה של דרישות לקוח לפי תנאי מסויים
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<GuestRequest> GetGuestRequestsByCondition(Predicate<GuestRequest> predicate)
        {
            return dal.GetGuestRequests().FindAll(predicate);
        }

        public int GetAmountOrders(GuestRequest guestRequest)
        {
            return (from item in dal.GetOrders()
                    where item.GuestRequestKey == guestRequest.guestRequestKey && item.Status == OrderStatus.נשלח_מייל
                    select true).Count();
        }

        public int GetAmountOrders(HostingUnit hostingUnit)
        {
            return (from item in dal.GetOrders()
                    where item.HostingUnitKey == hostingUnit.HostingUnitKey
                    && (item.Status == OrderStatus.נשלח_מייל || item.Status == OrderStatus.נסגר_בהיענות_של_הלקוח)
                    select true).Count();
        }


        public IEnumerable<IGrouping<bool, GuestRequest>> GetGuestRequestsGroupByArea(Regions area)
        {
            return dal.GetGuestRequests().GroupBy(item => item.Area == area);
        }
        public IEnumerable<IGrouping<Regions, GuestRequest>> GetGuestRequestsGroupByArea()
        {
            return dal.GetGuestRequests().GroupBy(item => item.Area);
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GetGuestRequestsGroupByVacationersNumber()
        {
            return from item in dal.GetGuestRequests()
                   group item by (item.Adults + item.Children);
        }

        public IEnumerable<IGrouping<int, Host>> GetHostsGroupByNumOfUnits()
        {
            return from host in dal.GetHosts()
                   group host by host.NumOfHostingUnits;
        }

        public IEnumerable<IGrouping<bool, HostingUnit>> GetHostingUnitsGroupByArea(Regions area)
        {
            return from hu in dal.GetHostingUnits()
                   group hu by (hu.Area == area);
        }
        public IEnumerable<IGrouping<Regions, HostingUnit>> GetHostingUnitsGroupByArea()
        {
            return from hu in dal.GetHostingUnits()
                   group hu by (hu.Area);
        }
        public IEnumerable<IGrouping<BankBranch, Host>> GetHostsGroupByBankBranch()
        {
            return from host in dal.GetHosts()
                   group host by host.BankAccountDetails;
        }

        public HostingUnit GetHostingUnit(int key)
        {
            return dal.GetHostingUnits().Where(k => k.HostingUnitKey == key).FirstOrDefault();
        }

        public List<HostingUnit> GetHostingUnitsByOwner(string key)
        {
            var v= dal.GetHostingUnits().Where(k => k.OwnerKey == key);
            if(v.Any())
                return v.ToList();
            return new List<HostingUnit>();
        }

        public Order GetOrder(int key)
        {
            return dal.GetOrders().Where(k => k.OrderKey == key).FirstOrDefault();
        }

        public string AddHost(Host host)
        {
            if (!Configuration.BanksXmlFinish)
                throw new TypeUnloadedException("בודק את פרטי הבנק. אנא המתן.");
            if (!ValidateBankDetails(host.BankAccountDetails))
                throw new ArgumentException("פרטי חשבון בנק אינם תקינים");
            return dal.AddHost(host);
        }

        public void UpdateHost(Host host)
        {
            Host owner = dal.GetHosts().FirstOrDefault(h => host.HostKey == h.HostKey);

            if (owner.CollectionClearance && !host.CollectionClearance)
            {
                var v = from order in dal.GetOrders()
                        from hostingUnit in dal.GetHostingUnits()
                        where order.HostingUnitKey == hostingUnit.HostingUnitKey
                        && hostingUnit.OwnerKey == host.HostKey
                        && (order.Status == OrderStatus.טרם_טופל || order.Status == OrderStatus.נשלח_מייל)
                        select order;

                if (v.Any())
                    throw new ArgumentException("לא ניתן לבטל הרשאה לחיוב חשבון כאשר יש הזמנות פתוחות");
            }
            dal.UpdateHost(host);
        }

        public Host GetHost(string key)
        {
            return dal.GetHosts().Where(h => h.HostKey == key).FirstOrDefault();
        }

        public List<Order> GetOrdersByCondition(Predicate<Order> predicate)
        {
            return dal.GetOrders().FindAll(predicate);
        }

        public List<Order> GetOrdersByHostKey(string key)
        {
            return (from hu in GetHostingUnitsByOwner(key)
                    from order in dal.GetOrders()
                    where order.HostingUnitKey == hu.HostingUnitKey
                    select order).ToList<Order>();
        }     
        /// <summary>
        /// פונקציה שבודקת אם פרטי הבנק תקינים(קיימים במאגר)
        /// </summary>
        /// <param name="bb">פרטי בנק</param>
        /// <returns></returns>
        private bool ValidateBankDetails(BankBranch bb)
        {
            var v = from item in dal.GetBankBranches()
                    where item.BankName == bb.BankName
                    && item.BankNumber == bb.BankNumber
                    && item.BranchAddress == bb.BranchAddress
                    && item.BranchCity == bb.BranchCity
                    && item.BranchNumber == bb.BranchNumber
                    select item;
            return v.Any();
        }
    }
}
