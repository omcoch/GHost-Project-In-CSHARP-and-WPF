using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DalFactory
    {
        public static IDAL getDal()
        {
            return Dal_xml_imp.GetInstance();
        }
    }
}
