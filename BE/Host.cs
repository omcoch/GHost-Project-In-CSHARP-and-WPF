using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class Host
    {
        public int HostKey { get; set; }
        public string PrivateName{ get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public BankAccount BankAccountDetails { get; set; }
        public int BankAccountNumber { get; set; }
        public bool CollectionClearance { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
