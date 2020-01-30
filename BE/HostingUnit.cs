using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utilities;

namespace BE
{
    [XmlRoot("HostingUnits")]
    public class HostingUnit
    {
        public int HostingUnitKey { get; set; }
        public int OwnerKey { get; set; }
        public string HostingUnitName { get; set; }
        [XmlIgnore]
        public bool[,] Diary { get; set; }
        //optional. tell the XmlSerializer to name the Array Element as'Board' instead of 'BoaredDto'
        [XmlArray("Diary")]
        public bool[] DiaryArray
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(12); } //5 is the number of roes in the matrix
        }
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
            string str ="מספר זהות בעל היחידה: " + OwnerKey +
                "\nשם יחידת אירוח: " + HostingUnitName +
                "\nאזור: " + Area + " תת אזור: " + SubArea +
                "\nסוג: " + Type +
                "\nמספר מבוגרים: " + Adults + " וילדים: " + Children +
                "\nיש בריכה? " + (Pool ? "כן" : "לא") +
                "\nיש ג'קוזי? " + (Jacuzzi ? "כן" : "לא") +
                "\nיש גינה? " + (Garden ? "כן" : "לא") +
                "\nיש אטרקציות לילדים? " + (ChildrensAttractions ? "כן" : "לא");
            return str;
        }
    }
}
