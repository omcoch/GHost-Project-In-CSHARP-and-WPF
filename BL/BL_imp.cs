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
        IDAL dal;
        public BL_imp()
        {
            dal = DalFactory.getDal();
        }
        public void AddGuestRequest(GuestRequest guestRequest)
        {
            TimeSpan timeSpan = guestRequest.ReleaseDate - guestRequest.EntryDate;
            if (timeSpan.TotalDays < 1)
                throw new TaskCanceledException("על תאריך תחילת הנופש להיות קודם לפחות ביום אחד לתאריך סיום הנופש");
            /*if (!new EmailAddressAttribute().IsValid(guestRequest.MailAddress))
                throw new FormatException("כתובת מייל לא תקינה");*/
            
        }
           

        public void AddHostingUnit(HostingUnit hostingUnit)
        {
            throw new NotImplementedException();
        }

        public void AddOrder(Order order)
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

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            
        }
    }
}
