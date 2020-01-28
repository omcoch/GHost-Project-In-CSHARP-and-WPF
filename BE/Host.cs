using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class Host
    {
        public int HostKey { get; set; }//TODO: לשנות לסטרינג
        public string PrivateName{ get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }
        [XmlIgnore]
        public MailAddress MailAddress { get; set; }
        [XmlElement("MailAddress")]
        public string mailAddress
        {
            get { return MailAddress.Address; }
            set { MailAddress = (MailAddress)value; } //5 is the number of roes in the matrix
        }
        public BankBranch BankAccountDetails { get; set; }
        public int BankAccountNumber { get; set; }
        public int ChargeAmount { get; set; }
        public bool CollectionClearance { get; set; }
        public int NumOfHostingUnits { get; set; }

        public override string ToString()
        {
            string str = "מספר זהות: " + HostKey +
                "\nשם: " + PrivateName + " " + FamilyName +
                "\nמספר פלאפון: " + PhoneNumber + " כתובת מייל: " + MailAddress.Address +
                "\nפרטי חשבון בנק: " +
                "\nמספר חשבון:" + BankAccountNumber + " " + BankAccountDetails.BankName + "מספר בנק  " + BankAccountDetails.BankNumber +
                "\nמספר סניף " + BankAccountDetails.BranchNumber + " כתובת סניף: " + BankAccountDetails.BranchAddress + " " + BankAccountDetails.BranchCity +
                "\nהאם אישר חיוב חשבון: " + (CollectionClearance ? "כן" : "לא")+" סכום לחיוב: " + ChargeAmount +
                "\nמספר יחידות אירוח בבעלותו: "+ NumOfHostingUnits;
            return str;
        }
    }
}
