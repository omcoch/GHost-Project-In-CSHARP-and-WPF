using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BlFactory
    {
        public static IBL getBl()
        {
            return BL_imp.GetInstance();
        }
    }
}
