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
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }
        public bool[,] Diary { get; set; }

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
            return base.ToString();
        }
    }
}
