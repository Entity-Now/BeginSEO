using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils._5118.Data
{
    public enum _5118Code
    {
        Success,
        Error
    }
    public class _5118Result
    {
        public _5118Code code { get; set; }
        public string msg { get; set; }
        public double like { get; set; }
    }
}
