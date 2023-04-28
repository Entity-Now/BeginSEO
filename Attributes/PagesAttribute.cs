using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Attributes
{
    public class PagesAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsHome { get; set; }
        public int Orderby { get; set; }
        public PagesAttribute(string name, int orderby = 1000, bool IsHome = false) 
        { 
            Name = name;
            this.Orderby = orderby;
            this.IsHome = IsHome;
        }
    }
}
