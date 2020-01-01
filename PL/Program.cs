using System;
using DAL;
namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Write(new EmailAddressAttribute().IsValid(""));
           
        }
    }
}
