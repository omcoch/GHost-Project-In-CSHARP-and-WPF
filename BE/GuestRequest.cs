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
        public string MailAddress { get; set; }
        public RequestStatus Status;
        public DateTime RegistrationDate { get; set; }              
        public DateTime EntryDate { get; set; }              
        public DateTime ReleaseDate { get; set; }              
        public Regions Area { get; set; }              
        public string SubArea { get; set; }              
        public Type Type { get; set; }       
        public int Adults { get; set; }
        public int Children { get; set; }
        public Pool Pool { get; set; }
        public HotTub Jacuzzi { get; set; }
        public Garden Garden { get; set; }
        public ChildrensAttractions ChildrensAttractions { get; set; }

        public override string ToString()
        {
            string str = "Name: " + PrivateName + " " + FamilyName +
                "\nStatus: " + Status +
                "\nRegistration date: " + RegistrationDate +
                "\nEntry date: " + EntryDate + " Release date: " + ReleaseDate +
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
