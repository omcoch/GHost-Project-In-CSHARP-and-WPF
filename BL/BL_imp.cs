using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public void AddGuestRequest(GuestRequest guestRequest)
        {
            try
            {
                TimeSpan timeSpan = guestRequest.ReleaseDate - guestRequest.EntryDate;
                if (timeSpan.TotalDays < 1)
                    throw new ArgumentOutOfRangeException("על תאריך תחילת הנופש להיות קודם לפחות ביום אחד לתאריך סיום הנופש");
                /*TODO: if (!new EmailAddressAttribute().IsValid(guestRequest.MailAddress))
                    throw new FormatException("כתובת מייל לא תקינה");*/
                dal.AddGuestRequest(guestRequest);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
           

        public void AddHostingUnit(HostingUnit hostingUnit)
        {
            
        }

        public void AddOrder(Order order)
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

                while (entryDate <= releaseDate)
                {
                    if (diaryOfHostingUnit[entryDate.Day, entryDate.Month])
                        throw new DateOccupiedException("התאריך תפוס") { Source = "BL" };
                    entryDate = entryDate.AddDays(1);
                }
                dal.AddOrder(order);
            }
            else
                throw new ExecutionOrderException("דרישת לקוח/יחידת אירוח לא קיימת") { Source = "BL" };
        }
        

        public void RemoveHostingUnit(int key)
        {
            throw new NotImplementedException();
        }

        public void UpdateGuestRequest(GuestRequest guestRequest)
        {
            throw new NotImplementedException();
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order Order)
        {
            var host = (from h in dal.GetHostingUnits()
                         where h.HostingUnitKey == Order.HostingUnitKey
                         select h).FirstOrDefault();
            var OrderStatus = (from order in dal.GetOrders()
                               where order.OrderKey == Order.OrderKey
                               select order.Status).FirstOrDefault();
            
            if (OrderStatus == OrderStatus.נסגר_בהיענות_של_הלקוח || OrderStatus == OrderStatus.נסגר_מחוסר_הענות_של_הלקוח)
                throw new ExecutionOrderException("הבקשה כבר סגורה");

            if (host.Owner.CollectionClearance)
                try
                {
                    dal.UpdateOrder(Order);
                    if (Order.Status == OrderStatus.נסגר_בהיענות_של_הלקוח)
                    {
                        var v = (from GR in dal.GetGuestRequests()
                                 where GR.guestRequestKey == Order.GuestRequestKey
                                 select GR).FirstOrDefault();
                        // במידה ונסגרה ההעסקה בינהם נבצע חישוב עמלה
                        host.Owner.ChargeAmount += CalculateFee((v.ReleaseDate - v.EntryDate).Days);
                        // מילוי המטריצה בתאריכים המבוקשים
                        FillDiary(host, v.EntryDate, v.ReleaseDate);
                        // עדכון יחידת האירוח בבסיס הנתונים
                        dal.UpdateHostingUnit(host);

                        // עדכון סטטוס דרישת לקוח
                        v.Status = RequestStatus.נסגרה_דרך_האתר;
                        dal.UpdateGuestRequest(v);

                        // עדכון שאר ההזמנות של אותה דרישת לקוח כסגורות
                        var orders = from o in dal.GetOrders()
                                     where o.GuestRequestKey == v.guestRequestKey&&o.OrderKey!=Order.OrderKey
                                     select o;
                        foreach(var order in orders)
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
        


        int CalculateFee(int amountDays)
        {
            return amountDays * Configuration.FEE;  
        }

        void FillDiary(HostingUnit hostingUnit,DateTime entry,DateTime release)
        {
            while(entry<=release)
            {
                hostingUnit.Diary[entry.Day, entry.Month] = true;
                entry.AddDays(1);
            }
        }
    }
}
