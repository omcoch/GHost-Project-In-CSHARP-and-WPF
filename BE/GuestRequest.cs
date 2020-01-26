using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

        

namespace BE
{
    public class GuestRequest
    {
        public int guestRequestKey
        {
            get;
            set;
        } // מספר הבקשה לאירוח - מזהה ייחודי

        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public MailAddress MailAddress { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }              
        public DateTime EntryDate { get; set; }              
        public DateTime ReleaseDate { get; set; }              
        public Regions Area { get; set; }              
        public string SubArea { get; set; }              
        public GRType Type { get; set; }       
        public int Adults { get; set; }
        public int Children { get; set; }
        public Requirements Pool { get; set; }
        public Requirements Jacuzzi { get; set; }
        public Requirements Garden { get; set; }
        public Requirements ChildrensAttractions { get; set; }
        public double MaxPrice { get; set; } // מחיר מקסמלי עבור חדר מבוקש

        public override string ToString()
        {
            string str = "Name: " + PrivateName + " " + FamilyName +
                "\nStatus: " + Status +
                "\nRegistration date: " + RegistrationDate.ToShortDateString() +
                "\nEntry date: " + EntryDate.ToShortDateString() + " Release date: " + ReleaseDate.ToShortDateString() +
                "\nArea: " + Area + " sub area: " + SubArea +
                "\nType: " + Type +
                "\nNumber of adults: " + Adults + " and childrens: " + Children +
                "\nWant Pool: " + Pool +
                "\nWant Jacuzzi: " + Jacuzzi +
                "\nWant Garden: " + Garden +
                "\nWant Childrens Attractions: " + ChildrensAttractions;
            return str;
        }
    }
}
