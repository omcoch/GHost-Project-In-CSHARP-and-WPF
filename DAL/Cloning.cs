using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Cloning
    {
        public static GuestRequest Clone(GuestRequest guestRequest)
        {
            GuestRequest GR = new GuestRequest();
            GR.Adults = guestRequest.Adults;
            GR.Area = guestRequest.Area;
            GR.Children = guestRequest.Children;
            GR.ChildrensAttractions = guestRequest.ChildrensAttractions;
            GR.EntryDate = guestRequest.EntryDate;
            GR.FamilyName = guestRequest.FamilyName;
            GR.Garden = guestRequest.Garden;
            GR.guestRequestKey = guestRequest.guestRequestKey;
            GR.Jacuzzi = guestRequest.Jacuzzi;
            GR.MailAddress = guestRequest.MailAddress;
            GR.Pool = GR.Pool;
            GR.PrivateName = guestRequest.PrivateName;
            GR.RegistrationDate = guestRequest.RegistrationDate;
            GR.ReleaseDate = guestRequest.ReleaseDate;
            GR.Status = guestRequest.Status;
            GR.SubArea = guestRequest.SubArea;
            GR.Type = guestRequest.Type;
            return GR;
        }
        public static HostingUnit Clone(HostingUnit hostingUnit)
        {
            HostingUnit hu = new HostingUnit()
            {
                Owner = new Host()
                {
                    HostKey = hostingUnit.Owner.HostKey,
                    PrivateName = hostingUnit.Owner.PrivateName,
                    FamilyName = hostingUnit.Owner.FamilyName,
                    PhoneNumber = hostingUnit.Owner.PhoneNumber,
                    MailAddress = hostingUnit.Owner.MailAddress,
                    BankAccountDetails = hostingUnit.Owner.BankAccountDetails,
                    BankAccountNumber = hostingUnit.Owner.BankAccountNumber,
                    ChargeAmount = hostingUnit.Owner.ChargeAmount,
                    CollectionClearance = hostingUnit.Owner.CollectionClearance,
                    NumOfHostingUnits = hostingUnit.Owner.NumOfHostingUnits,
                },
                HostingUnitName = hostingUnit.HostingUnitName,
                Area = hostingUnit.Area,
                SubArea = hostingUnit.SubArea,
                ChildrensAttractions = hostingUnit.ChildrensAttractions,
                Garden = hostingUnit.Garden,
                Jacuzzi = hostingUnit.Jacuzzi,
                Pool = hostingUnit.Pool,
                Children = hostingUnit.Children,
                Adults = hostingUnit.Adults,
                Type = hostingUnit.Type,
                HostingUnitKey = hostingUnit.HostingUnitKey,
                Diary = new bool[12,31]
            };
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 31; j++)
                    hu.Diary[i, j] = hostingUnit.Diary[i, j];
            return hu;
        }
        public static Order Clone(Order order)
        {
            return new Order()
            {
                CreateDate = order.CreateDate,
                GuestRequestKey = order.GuestRequestKey,
                HostingUnitKey = order.HostingUnitKey,
                OrderDate = order.OrderDate,
                OrderKey = order.OrderKey,
                Status = order.Status
            };
        }
    }
}
