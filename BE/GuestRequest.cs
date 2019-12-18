using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: להוסיף קישורים \ רפרנסים בין השכבות השונות

             

namespace BE
{
    public class GuestRequest
    {
        private readonly int guestRequestKey; // מספר הבקשה לאירוח - מזהה ייחודי
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

        }
    }
}
