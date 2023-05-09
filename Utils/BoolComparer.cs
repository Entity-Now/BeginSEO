using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils
{
    public class BoolComparer : IComparer<bool>
    {
        public int Compare(bool x, bool y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

}
