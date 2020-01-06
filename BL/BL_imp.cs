﻿using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;

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

            try
            {
                return dal.AddGuestRequest(guestRequest);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
           

        public int AddHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                hostingUnit.Owner.NumOfHostingUnits++;//todo: לטפל במקרה שלא הצליח להוסיף אז לעשות --
                return dal.AddHostingUnit(hostingUnit);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public int AddOrder(Order order)
        {
            var diaryOfHostingUnit =(from hu in dal.GetHostingUnits()
                                    where hu.HostingUnitKey == order.HostingUnitKey
                                    select hu.Diary).FirstOrDefault();
            var guestRequestDates = dal.GetGuestRequests()
                .Where(x => x.guestRequestKey == order.GuestRequestKey)
                .Select(x => new { entryDate = x.EntryDate, releaseDate = x.ReleaseDate }).FirstOrDefault();

            if (null != guestRequestDates && null != diaryOfHostingUnit)
            {
                DateTime entryDate = guestRequestDates.entryDate,
                    releaseDate = guestRequestDates.releaseDate;

                if(!CheckDiary(entryDate,releaseDate, diaryOfHostingUnit))
                    throw new DateOccupiedException("התאריך תפוס") { Source = "BL" };

                try
                {
                    return dal.AddOrder(order);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            else
                throw new ExecutionOrderException("דרישת לקוח/יחידת אירוח לא קיימת") { Source = "BL" };
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
                try
                {
                    dal.RemoveHostingUnit(key);
                }
                catch (KeyNotFoundException e)
                {

                    throw e;
                }
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

            try
            {
                dal.UpdateGuestRequest(guestRequest);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            HostingUnit originalHostingUnit = dal.GetHostingUnits().FirstOrDefault(item => item.HostingUnitKey == hostingUnit.HostingUnitKey);
            if (originalHostingUnit.Owner.CollectionClearance && !hostingUnit.Owner.CollectionClearance)
            {
                var v = from order in dal.GetOrders()
                        where order.HostingUnitKey == hostingUnit.HostingUnitKey
                        && (order.Status == OrderStatus.טרם_טופל || order.Status == OrderStatus.נשלח_מייל)
                        select order;
                if (v.Any())                    
                    throw new ArgumentException("לא ניתן לבטל הרשאה לחיוב חשבון כאשר יש הזמנות פתוחות");
            }
            try
            {
                dal.UpdateHostingUnit(hostingUnit);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public void UpdateOrder(Order Order)
        {
            // יחידת האירוח הקשורה להזמנה
            var host = (from h in dal.GetHostingUnits()
                        where h.HostingUnitKey == Order.HostingUnitKey
                        select h).FirstOrDefault();
            // סטטוס ההזמנה כפי ששמור במקור הנתונים
            var os = from order in dal.GetOrders()
                     where order.OrderKey == Order.OrderKey
                     select order.Status;

            var guestRequest = (from GR in dal.GetGuestRequests()
                     where GR.guestRequestKey == Order.GuestRequestKey
                     select GR).FirstOrDefault();

            if (!os.Any())
                throw new ArgumentException("ההזמנה לא קיימת במקור הנתונים");

            OrderStatus orderStatus = os.First();
            if (orderStatus == OrderStatus.נסגר_בהיענות_של_הלקוח || orderStatus == OrderStatus.נסגר_מחוסר_הענות_של_הלקוח)
                throw new ExecutionOrderException("הבקשה כבר סגורה");

            if (host.Owner.CollectionClearance)
                try
                {
                    if (!CheckDiary(guestRequest.EntryDate, guestRequest.ReleaseDate, host.Diary))
                        throw new DateOccupiedException("תאריך הנופש תפוס");
                    if (Order.Status == OrderStatus.נשלח_מייל)
                    {
                        SendMailToGuest(Order); //todo: הUI צריך לשנות את OrderDate למתי שהמייל נשלח (או לא נשלח) 
                        Order.OrderDate = DateTime.Now;
                    }
                     // עדכון ההזמנה
                    dal.UpdateOrder(Order);

                    

                    // עדכון הזמנה כאשר האירוח התקיים
                    if (Order.Status == OrderStatus.נסגר_בהיענות_של_הלקוח)
                    {

                        // במידה ונסגרה ההעסקה בינהם נבצע חישוב עמלה
                        host.Owner.ChargeAmount += CalculateFee((guestRequest.ReleaseDate - guestRequest.EntryDate).Days);
                        // מילוי המטריצה בתאריכים המבוקשים
                        FillDiary(host, guestRequest.EntryDate, guestRequest.ReleaseDate);
                        // עדכון יחידת האירוח בבסיס הנתונים
                        dal.UpdateHostingUnit(host);

                        // עדכון סטטוס דרישת לקוח
                        guestRequest.Status = RequestStatus.נסגרה_דרך_האתר;
                        dal.UpdateGuestRequest(guestRequest);

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

        private bool SendMailToGuest(Order order)
        {
            MailAddress mail_address = dal.GetGuestRequests().Where(gr => gr.guestRequestKey == order.GuestRequestKey).Select(gr => gr.MailAddress).First();
            return Tools.SendMail(mail_address);
        }

        int CalculateFee(int amountDays)
        {
            return amountDays * Configuration.FEE;  
        }

        void FillDiary(HostingUnit hostingUnit,DateTime entry,DateTime release)
        {
            while(entry<=release)
            {
                hostingUnit.Diary[entry.Month, entry.Day] = true;
                entry = entry.AddDays(1);
            }
        }

        public List<HostingUnit> GetAvailableHostingUnits(DateTime date, int days)
        {
            var l = (from unit in dal.GetHostingUnits()
                    where CheckDiary(date, date.AddDays(days), unit.Diary) == true
                    select unit).ToList();
            return l;
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
                .FindAll(delegate(Order order)
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


        public IEnumerable<IGrouping<Regions, GuestRequest>> GetGuestRequestsGroupByArea(Regions area)
        {
            return dal.GetGuestRequests().GroupBy(item => item.Area==area?area:0);
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GetGuestRequestsGroupByVacationersNumber()
        {
            return from item in dal.GetGuestRequests()
                   group item by (item.Adults + item.Children); 
        }

        public IEnumerable<IGrouping<int, Host>> GetHostsGroupByNumOfUnits()
        {
            return from hu in dal.GetHostingUnits()
                   group hu.Owner by hu.Owner.NumOfHostingUnits;
        }

        public IEnumerable<IGrouping<Regions, HostingUnit>> GetHostingUnitsGroupByArea(Regions area)
        {
            return from hu in dal.GetHostingUnits()
                   group hu by (hu.Area==area)?area:0;
        }

        public IEnumerable<IGrouping<BankBranch, Host>> GetHostsGroupByBankBranch()
        {
            return from hu in dal.GetHostingUnits()
                   group hu.Owner by hu.Owner.BankAccountDetails;
        }

        public HostingUnit GetHostingUnit(int key)
        {
            return dal.GetHostingUnits().Where(k => k.HostingUnitKey == key).FirstOrDefault();
        }

        public Order GetOrder(int key)
        {
            return dal.GetOrders().Where(k => k.OrderKey == key).FirstOrDefault();
        }





        //todo: לבדןוק CLONE לגבי FIRST, FIND ETC...
    }
}
