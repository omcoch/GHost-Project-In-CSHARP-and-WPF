using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        public int HostingUnitKey { get; set; }
        public int OwnerKey { get; set; }
        public string HostingUnitName { get; set; }
        public bool[,] Diary { get; set; }

        public Regions Area { get; set; }
        public string SubArea { get; set; }
        public GRType Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool ChildrensAttractions { get; set; }
        public double Price { get; set; } // מחיר עבור יחידת אירוח    

        public override string ToString()
        {
            string str = 
                "Owner ID: " + OwnerKey +
                "\nHosting Unit Name: " + HostingUnitName +
                "\nArea: " + Area + " sub area: " + SubArea +
                "\nType: " + Type +
                "\nNumber of adults: " + Adults + " and childrens: " + Children +
                "\nHave Pool? " + Pool +
                "\nHave Jacuzzi? " + Jacuzzi +
                "\nHave Garden? " + Garden +
                "\nHave Childrens Attractions? " + ChildrensAttractions;
            return str;
        }
    }
}
