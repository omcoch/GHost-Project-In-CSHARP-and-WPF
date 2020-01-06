using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        public int HostKey { get; set; }
        public string PrivateName{ get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }
        public MailAddress MailAddress { get; set; }
        public BankBranch BankAccountDetails { get; set; }
        public int BankAccountNumber { get; set; }
        public int ChargeAmount { get; set; }
        public bool CollectionClearance { get; set; }
        public int NumOfHostingUnits { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
