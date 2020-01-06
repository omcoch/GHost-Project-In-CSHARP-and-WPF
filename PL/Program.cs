using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;
namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL bL = BlFactory.getBl();
            int choice;

            GuestRequest GR = new GuestRequest();
            HostingUnit HU = new HostingUnit();
            Host H = new Host();
            Order O = new Order();
            
            
            do
            {
                Console.WriteLine(" MAIN MENU:\n1: Guest Request \n2: Hosting Unit \n3: Order \n4 Get Hosting Units Grouping By Area");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    // Guest Request
                    case 1:
                        Console.WriteLine("1: Add Guest Request \n2: Update Guest Request");
                        switch (int.Parse(Console.ReadLine()))
                        {
                            case 1:
                                Console.WriteLine("Enter : PrivateName, FamilyName, Mail Address, Entry date, Release date , Area, Sub area, Type , Number of adults, of childrens, Pool, Jacuzzi, Garden, ChildrensAttractions:");

                                GR = new GuestRequest()
                                {
                                    PrivateName = Console.ReadLine(),
                                    FamilyName = Console.ReadLine(),
                                    MailAddress = new MailAddress(Console.ReadLine()),
                                    Status = RequestStatus.פתוחה,
                                    RegistrationDate = DateTime.Now,
                                    EntryDate = DateTime.Parse(Console.ReadLine()),
                                    ReleaseDate = DateTime.Parse(Console.ReadLine()),
                                    Area = (Regions)Enum.Parse(typeof(Regions), Console.ReadLine()),
                                    SubArea = Console.ReadLine(),
                                    Type = (GRType)Enum.Parse(typeof(GRType), Console.ReadLine()),
                                    Adults = int.Parse(Console.ReadLine()),
                                    Children = int.Parse(Console.ReadLine()),
                                    Pool = (Requirements)Enum.Parse(typeof(Requirements), Console.ReadLine()),
                                    Jacuzzi = (Requirements)Enum.Parse(typeof(Requirements), Console.ReadLine()),
                                    Garden = (Requirements)Enum.Parse(typeof(Requirements), Console.ReadLine()),
                                    ChildrensAttractions = (Requirements)Enum.Parse(typeof(Requirements), Console.ReadLine()),
                                };
                                try
                                {
                                    GR.guestRequestKey = bL.AddGuestRequest(GR);
                                    Console.WriteLine(bL.GetGuestRequestsByCondition(item => item.guestRequestKey == GR.guestRequestKey));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message+ "\nSource: " + e.Source);
                                }
                                break;
                            case 2:
                                GR.PrivateName = "abcde"; //  שם שבטוח לא הכנסת
                                try
                                {
                                    bL.UpdateGuestRequest(GR);
                                    Console.WriteLine(bL.GetGuestRequestsByCondition(item => item.guestRequestKey == GR.guestRequestKey));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nSource: " + e.Source);
                                }
                                break;
                        }
                        break;

                    // Hosting Unit
                    case 2:
                        Console.WriteLine("1: Add Hosting Unit \n2: Update Hosting Unit \n3: Remove Hosting Unit");
                        switch (int.Parse(Console.ReadLine()))
                        {
                            case 1:
                                Console.WriteLine("ENTER: ");
                                HU = new HostingUnit()
                                {
                                    Owner = new Host()
                                    {
                                        HostKey = 987654321,
                                        PrivateName = "רון",
                                        FamilyName = "קובי",
                                        PhoneNumber = "054-1111111",
                                        MailAddress = new MailAddress("ron@kubi.com"),
                                        BankAccountDetails = new BankBranch() { BankNumber = 12, BankName = "בנק הפועלים", BranchNumber = 723, BranchAddress = "הגליל", BranchCity = "טבריה" },
                                        BankAccountNumber = 123456,
                                        ChargeAmount = 0,
                                        CollectionClearance = true,
                                        NumOfHostingUnits = 1,
                                    },
                                    HostingUnitName = "נווה מחמד",
                                    Area = Regions.ירושלים,
                                    SubArea = "מכון לב",
                                    ChildrensAttractions = true,
                                    Garden = false,
                                    Jacuzzi = true,
                                    Pool = true,
                                    Children = 0,
                                    Adults = 2,
                                    Type = GRType.מלון,
                                    Diary = new bool[12, 31]
                                };

                                HU.HostingUnitKey = bL.AddHostingUnit(HU);

                                Console.WriteLine(bL.GetHostingUnit(HU.HostingUnitKey));
                                break;

                            case 2:
                                HU.Adults = 5;
                                HU.Pool = false;
                                Console.WriteLine(bL.GetHostingUnit(HU.HostingUnitKey));
                                break;
                            case 3:
                                bL.RemoveHostingUnit(HU.HostingUnitKey);
                                break;
                        }
                        break;

                    // Order
                    case 3:
                        Console.WriteLine("1: Add Order \n2: Update Order");
                        switch (int.Parse(Console.ReadLine()))
                        {
                            case 1:
                                Console.WriteLine("ENTER: ");
                                O = new Order()
                                {
                                    GuestRequestKey = GR.guestRequestKey,
                                    HostingUnitKey = HU.HostingUnitKey,
                                    Status = OrderStatus.טרם_טופל,
                                    CreateDate = DateTime.Now,
                                };
                                try
                                {
                                    O.OrderKey = bL.AddOrder(O);
                                    Console.WriteLine(bL.GetOrder(O.OrderKey));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nSource: " + e.Source);
                                }
                                break;
                            case 2:
                                try
                                {
                                    O.Status = OrderStatus.נשלח_מייל;
                                    bL.UpdateOrder(O);
                                    Console.WriteLine(bL.GetOrder(O.OrderKey));
                                    O.Status = OrderStatus.נסגר_בהיענות_של_הלקוח;
                                    bL.UpdateOrder(O);
                                    Console.WriteLine(bL.GetOrder(O.OrderKey));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message + "\nSource: " + e.Source);
                                }
                                break;
                        }
                        break;

                    // Get Hosting Units Grouping By Area
                    case 4:
                        var hu = bL.GetHostingUnitsGroupByArea(Regions.דרום);
                        foreach(var item in hu)
                        {
                            switch(item.Key)
                            {
                                case Regions.דרום:
                                    Console.WriteLine("דרום:");
                                    foreach (var n in item)
                                        Console.WriteLine("{0}, ", n);
                                    break;
                                default:
                                    Console.WriteLine("כל השאר");
                                    foreach (var n in item)
                                        Console.Write("{0}, ", n);
                                    break;
                            }
                        }
                        break;
                }
            } while (choice != 0);
        }
    }
}
