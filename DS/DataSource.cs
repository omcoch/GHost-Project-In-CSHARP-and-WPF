using System;
using System.Collections.Generic;
using System.Net.Mail;
using BE;
namespace DS
{
    public class DataSource
    {
        public static List<GuestRequest> GuestRequests = new List<GuestRequest>() { new GuestRequest() { guestRequestKey=Configuration.GenerateGuestRequestSerialKey } };
        public static List<HostingUnit> HostingUnits = new List<HostingUnit>()
        {
            new HostingUnit()
            {
                HostingUnitKey = Configuration.GenerateHostingUnitSerialKey,
                Owner = new Host()
                {
                    HostKey = 312279188,
                    PrivateName = "איציק",
                    FamilyName = "שמואלי",
                    PhoneNumber = "054-222222",
                    MailAddress = new MailAddress("i@j.c"),
                    BankAccountDetails = new BankBranch() { BankNumber = 12, BankName = "בנק הפועלים", BranchNumber = 723, BranchAddress = "הגליל", BranchCity = "טבריה" },
                    BankAccountNumber = 654321,
                    ChargeAmount = 3,
                    CollectionClearance = true,
                    NumOfHostingUnits = 1,
                },
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
                Diary = new bool[12,31]
            },

        };
        public static List<Order> Orders = new List<Order>()
        {
            
        };
    }
}
