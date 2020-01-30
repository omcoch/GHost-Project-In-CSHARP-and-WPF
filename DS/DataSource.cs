using System;
using System.Collections.Generic;
using System.Net.Mail;
using BE;
namespace DS
{
    public class DataSource
    {
        public static List<GuestRequest> GuestRequests = new List<GuestRequest>() { new GuestRequest() { guestRequestKey=Configuration.GenerateGuestRequestSerialKey,Area=Regions.דרום }, new GuestRequest() { guestRequestKey = Configuration.GenerateGuestRequestSerialKey, Area = Regions.צפון } };
        public static List<HostingUnit> HostingUnits = new List<HostingUnit>()
        {
            new HostingUnit()
            {
                HostingUnitKey = Configuration.GenerateHostingUnitSerialKey,
                OwnerKey = "312279188",
                HostingUnitName = "מלון בראשית",
                Area = Regions.דרום,
                SubArea = "מצפה רמון",
                ChildrensAttractions = true,
                Garden = true,
                Jacuzzi = true,
                Pool = true,
                Children = 5,
                Adults = 5,
                Type = GRType.מלון,
                Diary = new bool[12,31],
                Price=1500 
            },

        };
        public static List<Order> Orders = new List<Order>()
        {
        };
        public static List<Host> Hosts = new List<Host>()
        {
            new Host()
                {
                    HostKey = "312279188",
                    PrivateName = "איציק",
                    FamilyName = "שמואלי",
                    PhoneNumber = "054222222",
                    MailAddress = new MailAddress("i@j.c"),
                    BankAccountDetails = new BankBranch() { BankNumber = 12, BankName = "בנק הפועלים", BranchNumber = 723, BranchAddress = "הגליל", BranchCity = "טבריה" },
                    BankAccountNumber = 654321,
                    ChargeAmount = 3,
                    CollectionClearance = true,
                    NumOfHostingUnits = 1,
                },
        };
    }
}
