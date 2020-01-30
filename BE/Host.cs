using System.Net.Mail;
using System.Xml.Serialization;

namespace BE
{
    public class Host
    {
        /// <summary> תז של המארח </summary>
        public string HostKey { get; set; }
        public string PrivateName{ get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }
        [XmlIgnore]
        public MailAddress MailAddress { get; set; }
        public string mailAddress
        {
            get => MailAddress.Address;
            set => MailAddress = new MailAddress(value);
        }
        public BankBranch BankAccountDetails { get; set; }
        public int BankAccountNumber { get; set; }
        public int ChargeAmount { get; set; }
        public bool CollectionClearance { get; set; } // האם קיים אישור לחיוב החשבון
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
