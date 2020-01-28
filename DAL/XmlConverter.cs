using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public static class XmlConverter
    {
        public static XElement ToXML(this Order order)
        {
            return new XElement("Order",
                new XElement("CreateDate",order.CreateDate),
                new XElement("GuestRequestKey",order.GuestRequestKey),
                new XElement("HostingUnitKey",order.HostingUnitKey),
                new XElement("OrderDate",order.OrderDate),
                new XElement("OrderKey",order.OrderKey),
                new XElement("Status",order.Status)
                );
        }

        public static XElement ToXML(this GuestRequest guestRequest)
        {
            return new XElement("GuestRequest",
                new XElement("Adults", guestRequest.Adults),
                new XElement("Area", guestRequest.Area),
                new XElement("Children", guestRequest.Children),
                new XElement("ChildrensAttractions", guestRequest.ChildrensAttractions),
                new XElement("EntryDate", guestRequest.EntryDate),
                new XElement("FamilyName", guestRequest.FamilyName),
                new XElement("Garden", guestRequest.Garden),
                new XElement("guestRequestKey", guestRequest.guestRequestKey),
                new XElement("Jacuzzi", guestRequest.Jacuzzi),
                new XElement("MailAddress", guestRequest.MailAddress),
                new XElement("Pool", guestRequest.Pool),
                new XElement("PrivateName", guestRequest.PrivateName),
                new XElement("RegistrationDate", guestRequest.RegistrationDate),
                new XElement("ReleaseDate", guestRequest.ReleaseDate),
                new XElement("Status", guestRequest.Status),
                new XElement("SubArea", guestRequest.SubArea),
                new XElement("Type", guestRequest.Type),
                new XElement("MaxPrice", guestRequest.MaxPrice)
            );
        }
        public static XElement ToXML(this Host host)
        {
            return new XElement("Host",
                new XElement("HostKey",host.HostKey),
                new XElement("PrivateName",host.PrivateName),
                new XElement("FamilyName",host.FamilyName),
                new XElement("PhoneNumber",host.PhoneNumber),
                new XElement("MailAddress",host.MailAddress),
                new XElement("BankAccountDetails",host.BankAccountDetails),
                new XElement("BankAccountNumber",host.BankAccountNumber),
                new XElement("ChargeAmount",host.ChargeAmount),
                new XElement("CollectionClearance",host.CollectionClearance),
                new XElement("NumOfHostingUnits",host.NumOfHostingUnits)
            );
        }
    }
}
