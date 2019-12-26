using System;
using DAL;
namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            IDAL d=DalFactory.getDal();
            d.AddGuestRequest(new BE.GuestRequest() { PrivateName = "hbf" });
            d.AddGuestRequest(new BE.GuestRequest() { PrivateName = "hbf" });
        }
    }
}
