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
        public PagesAttribute(string name) 
        { 
            Name = name;
        }
    }
}
