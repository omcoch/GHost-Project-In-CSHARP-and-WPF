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
            GuestRequest GR = new GuestRequest()
            {
                Adults = guestRequest.Adults,
                Area = guestRequest.Area,
                Children = guestRequest.Children,
                ChildrensAttractions = guestRequest.ChildrensAttractions,
                EntryDate = guestRequest.EntryDate,
                FamilyName = guestRequest.FamilyName,
                Garden = guestRequest.Garden,
                guestRequestKey = guestRequest.guestRequestKey,
                Jacuzzi = guestRequest.Jacuzzi,
                MailAddress = guestRequest.MailAddress,
                Pool = guestRequest.Pool,
                PrivateName = guestRequest.PrivateName,
                RegistrationDate = guestRequest.RegistrationDate,
                ReleaseDate = guestRequest.ReleaseDate,
                Status = guestRequest.Status,
                SubArea = guestRequest.SubArea,
                Type = guestRequest.Type,
                MaxPrice = guestRequest.MaxPrice
            };
            return GR;
        }
        public static HostingUnit Clone(HostingUnit hostingUnit)
        {
            HostingUnit hu = new HostingUnit()
            {
                OwnerKey = hostingUnit.OwnerKey,
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
                Diary = new bool[12, 31],
                Price = hostingUnit.Price
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
        public static Host Clone (Host host)
        {
            return new Host()
            {
                HostKey = host.HostKey,
                PrivateName = host.PrivateName,
                FamilyName = host.FamilyName,
                PhoneNumber = host.PhoneNumber,
                MailAddress = host.MailAddress,
                BankAccountDetails = host.BankAccountDetails,
                BankAccountNumber = host.BankAccountNumber,
                ChargeAmount = host.ChargeAmount,
                CollectionClearance = host.CollectionClearance,
                NumOfHostingUnits = host.NumOfHostingUnits,
            };
        }
    }
}
