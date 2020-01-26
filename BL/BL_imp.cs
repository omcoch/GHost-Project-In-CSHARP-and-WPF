using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
                owner.NumOfHostingUnits++;
                dal.UpdateHost(owner);
                return dal.AddHostingUnit(hostingUnit);
            }
            catch (ArgumentException e)
            {
                owner.NumOfHostingUnits--;
                dal.UpdateHost(owner);
                throw e;
            }
        }

        public int AddOrder(Order order)
        {
            var diaryOfHostingUnit = (from hu in dal.GetHostingUnits()
                                      where hu.HostingUnitKey == order.HostingUnitKey
                                      select hu.Diary).FirstOrDefault();
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
                if (diary[entryDate.Month, entryDate.Day])
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
        { // todo: לעשות בדיקה שלא התקבל מופע ריק לגמרי
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
            HostingUnit originalHostingUnit = dal.GetHostingUnits().FirstOrDefault(item => item.HostingUnitKey == hostingUnit.HostingUnitKey);
            dal.UpdateHostingUnit(hostingUnit);
        }

        public void UpdateOrder(Order Order)
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
            if (orderStatus == OrderStatus.נסגר_בהיענות_של_הלקוח || orderStatus == OrderStatus.נסגר_מחוסר_הענות_של_הלקוח)
                throw new ExecutionOrderException("הבקשה כבר סגורה");


            if (owner.CollectionClearance)
                try
                {
                    if (!CheckDiary(guestRequest.EntryDate, guestRequest.ReleaseDate, hostingUnit.Diary))
                        throw new DateOccupiedException("תאריך הנופש תפוס");
                    if (Order.Status == OrderStatus.נשלח_מייל)
                    {
                        SendMailToGuest(Order.GuestRequestKey, hostingUnit, guestRequest); //todo: הUI צריך לשנות את OrderDate למתי שהמייל נשלח (או לא נשלח) 
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
        /// שולח מייל ללקוח המעדכן אותו בדבר הזמנה שנוצרה עבורו
        /// </summary>
        /// <param name="orderKey">מספר הזמנה</param>
        /// <param name="guestRequest">דרישת לקוח</param>
        /// <returns>מחזיר אמת אם המייל נשלח בהצלחה, אחרת שקר</returns>
        private bool SendMailToGuest(int orderKey, HostingUnit hostingUnit, GuestRequest guestRequest)
        {
            MailAddress mailAddress = dal.GetHosts().Where(h => hostingUnit.OwnerKey == h.HostKey).Select(o => o.MailAddress).First();
            MailMessage message = new MailMessage();
            message.To.Add(guestRequest.MailAddress);
            message.Subject = "נוצרה הזמנה עבור דרישת לקוח מספר " + guestRequest.guestRequestKey;
            message.Body = "שלום, " + guestRequest.PrivateName + "\nנפתחנ עבורך הזמנה לאירוח אצל " + hostingUnit.HostingUnitName
                + ".\nמספר ההזמנה: " + orderKey + "\nאנא צור קשר עם המארח בכתובת " + mailAddress + "\nבברכת חופשה מהנה, \n" + Configuration.SiteName;
            return Tools.SendMail(message);
        }

        int CalculateFee(int amountDays)
        {
            return amountDays * Configuration.FEE;
        }

        void FillDiary(HostingUnit hostingUnit, DateTime entry, DateTime release)
        {
            while (entry <= release)
            {
                hostingUnit.Diary[entry.Month, entry.Day] = true;
                entry = entry.AddDays(1);
            }
        }

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

        public IEnumerable<IGrouping<BankBranch, Host>> GetHostsGroupByBankBranch()
        {
            return from host in dal.GetHosts()
                   group host by host.BankAccountDetails;
        }

        public HostingUnit GetHostingUnit(int key)
        {
            return dal.GetHostingUnits().Where(k => k.HostingUnitKey == key).FirstOrDefault();
        }

        public List<HostingUnit> GetHostingUnitsByOwner(int key)
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

        public int AddHost(Host host)
        {
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

        public Host GetHost(int key)
        {
            return dal.GetHosts().Where(h => h.HostKey == key).FirstOrDefault();
        }

        public List<Order> GetOrdersByCondition(Predicate<Order> predicate)
        {
            return dal.GetOrders().FindAll(predicate);
        }

        public List<Order> GetOrdersByHostKey(int key)
        {
            return (from hu in GetHostingUnitsByOwner(key)
                    from order in dal.GetOrders()
                    where order.HostingUnitKey == hu.HostingUnitKey
                    select order).ToList<Order>();
        }

        //todo: לבדןוק CLONE לגבי FIRST, FIND ETC...
    }
}
