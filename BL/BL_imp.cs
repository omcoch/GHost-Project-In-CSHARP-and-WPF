using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    throw new TaskCanceledException("על תאריך תחילת הנופש להיות קודם לפחות ביום אחד לתאריך סיום הנופש");
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
                                    select hu.Diary).ToList()[0];
            var guestRequestDates = dal.GetGuestRequests()
                .Where(x => x.guestRequestKey == order.GuestRequestKey)
                .Select(x => new { entryDate = x.EntryDate, releaseDate = x.ReleaseDate });
            DateTime entryDate = guestRequestDates.First().entryDate, releaseDate= guestRequestDates.First().releaseDate;
            
            while (entryDate<=releaseDate)
            {
                if (diaryOfHostingUnit[entryDate.Day, entryDate.Month])
                    throw new DateOccupiedException();
                entryDate = entryDate.AddDays(1);
            }
            dal.AddOrder(order);
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

        public void UpdateOrder(Order order)
        {
            var CollectionClearance = from host in dal.GetHostingUnits()
                    where host.HostingUnitKey == order.HostingUnitKey
                    select host.Owner.CollectionClearance;
            if (CollectionClearance.First() == true)
                try
                {
                    dal.UpdateOrder(order);
                }
                catch (Exception e)
                {
                    throw e;
                }
            else
                throw new TaskCanceledException("לא בוצע אישור לחיוב חשבון");
        }
    }
}
